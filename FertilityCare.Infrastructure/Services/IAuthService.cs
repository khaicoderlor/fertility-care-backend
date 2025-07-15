using FertilityCare.Domain.constants;
using FertilityCare.Domain.Entities;
using FertilityCare.Infrastructure.Configurations;
using FertilityCare.Infrastructure.Identity;
using FertilityCare.UseCase.DTOs.Auths;
using FertilityCare.UseCase.DTOs.Users;
using FertilityCare.UseCase.Interfaces.Repositories;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.Infrastructure.Services
{
    public interface IAuthService
    {
        Task<AuthResult> LoginAsync(LoginRequest request);

        Task<AuthResult> GoogleLoginAsync(GoogleLoginRequest request);

        Task<AuthResult> RefreshTokenAsync(RefreshTokenRequest request);

        Task<bool> LogoutAsync(string userId);

        Task<AuthResult> RegisterAsync(RegisterRequest request);

    }

    public class AuthService : IAuthService
    {

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly IOrderRepository _orderRepository;

        private readonly IPatientRepository _patientRepository;

        private readonly IDoctorRepository _doctorRepository;

        private readonly IJwtService _jwtService;

        private readonly JwtConfiguration _jwtConfig;

        private readonly GoogleAuthConfiguration _googleConfig;

        private readonly IUserProfileRepository _profileRepository;

        public AuthService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtService jwtService, IOptions<JwtConfiguration> jwtConfig,
            IOptions<GoogleAuthConfiguration> googleConfig,
            IPatientRepository patientRepository,
            IOrderRepository orderRepository, IUserProfileRepository profileRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _jwtConfig = jwtConfig.Value;
            _googleConfig = googleConfig.Value;
            _patientRepository = patientRepository;
            _orderRepository = orderRepository;
            _profileRepository = profileRepository;
        }

        public async Task<AuthResult> GoogleLoginAsync(GoogleLoginRequest request)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { _googleConfig.ClientId }
                });

                if (!payload.EmailVerified)
                    return AuthResult.Failed("Email chưa được xác minh bởi Google");

                var user = await _userManager.FindByEmailAsync(payload.Email);

                if (user == null)
                {
                    var profileId = Guid.NewGuid();

                    var newUser = new ApplicationUser
                    {
                        Id = Guid.NewGuid(),
                        UserName = payload.Email,
                        Email = payload.Email,
                        EmailConfirmed = true,
                        GoogleId = payload.Subject,
                        IsGoogleAccount = true,
                        RefreshToken = "",
                        UserProfileId = profileId,
                        UserProfile = new UserProfile
                        {
                            Id = profileId,
                            FirstName = payload.GivenName,
                            MiddleName = "",
                            LastName = payload.FamilyName,
                            AvatarUrl = payload.Picture,
                        }
                    };

                    var createResult = await _userManager.CreateAsync(newUser);
                    if (!createResult.Succeeded)
                        return AuthResult.Failed("Create account failed");

                    var roleResult = await _userManager.AddToRoleAsync(newUser, "User");
                    if (!roleResult.Succeeded)
                        return AuthResult.Failed("Not assign role to user");

                    newUser.LastLogin = DateTime.Now;
                    await _userManager.UpdateAsync(newUser);

                    Patient patient = new Patient
                    {
                        UserProfileId = profileId,
                        MedicalHistory = "",
                        PartnerEmail = "",
                        PartnerPhone = "",
                        PartnerFullName = ""
                    };

                    await _patientRepository.SaveAsync(patient);

                    return await GenerateTokenAsync(newUser);
                }

                if (!user.IsGoogleAccount)
                {
                    user.GoogleId = payload.Subject;
                    user.IsGoogleAccount = true;
                }

                user.LastLogin = DateTime.Now;
                await _userManager.UpdateAsync(user);

                return await GenerateTokenAsync(user);
            }
            catch (Exception ex)
            {
                return AuthResult.Failed("Google login failed");
            }
        }

        public async Task<AuthResult> LoginAsync(LoginRequest request)
        {
            try
            {
                var loadedUser = await _userManager.FindByEmailAsync(request.Email);
                if (loadedUser is null)
                {
                    return AuthResult.Failed("Invalid credentials!");
                }

                if (await _userManager.IsLockedOutAsync(loadedUser))
                {
                    return AuthResult.Failed("Account is locked on!");
                }

                var result = await _signInManager.CheckPasswordSignInAsync(loadedUser, request.Password, true);
                if (!result.Succeeded)
                {
                    if (result.IsLockedOut)
                    {
                        return AuthResult.Failed("Account locked due to multiple failed attempts");
                    }

                    return AuthResult.Failed("Invalid credentials");
                }

                loadedUser.LastLogin = DateTime.UtcNow;
                loadedUser.FailedLoginAttempts = 0;
                await _userManager.UpdateAsync(loadedUser);

                return await GenerateTokenAsync(loadedUser);
            }
            catch (Exception ex)
            {
                return AuthResult.Failed("Login failed");
            }
        }

        public async Task<bool> LogoutAsync(string userId)
        {
            try
            {
                if (!Guid.TryParse(userId, out var id))
                {
                    return false;
                }

                var loadedUser = await _userManager.FindByIdAsync(id.ToString());
                if (loadedUser is null)
                {
                    return false;
                }

                loadedUser.RefreshToken = null;
                loadedUser.RefreshTokenExpiryTime = null;

                await _userManager.UpdateAsync(loadedUser);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<AuthResult> RefreshTokenAsync(RefreshTokenRequest request)
        {
            try
            {
                var principal = _jwtService.GetPrincipalFromExpiredToken(request.AccessToken);
                if (principal is null)
                {
                    return AuthResult.Failed("Invalid access token");
                }

                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid id))
                {
                    return AuthResult.Failed("Invalid token claims");
                }

                var loadedUser = await _userManager.FindByIdAsync(userId);
                if (loadedUser == null || loadedUser.RefreshToken == request.RefreshToken || loadedUser.RefreshTokenExpiryTime <= DateTime.Now)
                {
                    return AuthResult.Failed("Invalid refresh token");
                }

                return await GenerateTokenAsync(loadedUser);
            }
            catch (Exception ex)
            {
                return AuthResult.Failed("Token refresh failed!");
            }
        }

        public async Task<AuthResult> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var loadedUser = await _userManager.FindByEmailAsync(request.Email);
                if (loadedUser is not null)
                {
                    return AuthResult.Failed("Email already registered");
                }

                if (!request.Password.Equals(request.ConfirmPassword, StringComparison.OrdinalIgnoreCase))
                {
                    return AuthResult.Failed("Confirm password not matches!");
                }

                var profileId = Guid.NewGuid();
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    Email = request.Email,
                    UserName = request.Email,
                    EmailConfirmed = true,
                    UserProfileId = profileId,
                    RefreshToken = "",
                    UserProfile = new UserProfile
                    {
                        Id = profileId,
                        FirstName = "None",
                        MiddleName = "",
                        LastName = "None",
                        Address = "",
                        CreatedAt = DateTime.Now,
                        AvatarUrl = ApplicationConstant.DefaultAvatar
                    }
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                {
                    return AuthResult.Failed("Register account failed!");
                }

                if (request.Role.Equals("User", StringComparison.OrdinalIgnoreCase))
                {
                    var roleAssignResult = await _userManager.AddToRoleAsync(user, "User");
                    if (!roleAssignResult.Succeeded)
                    {
                        return AuthResult.Failed("Not assign role to user");
                    }
                    else
                    {
                        Patient patient = new Patient
                        {
                            UserProfileId = profileId,
                            MedicalHistory = "",
                            PartnerEmail = "",
                            PartnerPhone = "",
                            PartnerFullName = ""
                        };

                        await _patientRepository.SaveAsync(patient);
                    }
                }
                else if (request.Role.Equals("Doctor", StringComparison.OrdinalIgnoreCase))
                {
                    var roleAssignResult = await _userManager.AddToRoleAsync(user, "Doctor");
                    if (!roleAssignResult.Succeeded)
                    {
                        return AuthResult.Failed("Not assign role to user");
                    }
                    else
                    {
                        Doctor doctor = new Doctor
                        {
                            UserProfileId = profileId,
                            Biography = "",
                            Degree = "",
                            PatientsServed = 0,
                            YearsOfExperience = 0,
                            Rating = 0,
                            Specialization = "",
                            UpdatedAt = DateTime.Now,
                        };

                        await _doctorRepository.SaveAsync(doctor);
                    }
                }
                else if (request.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    var roleAssignResult = await _userManager.AddToRoleAsync(user, "Admin");
                    if (!roleAssignResult.Succeeded)
                    {
                        return AuthResult.Failed("Not assign role to user");
                    }
                    
                    await _profileRepository.SaveChangeAsync();
                }
                else if (request.Role.Equals("Manager", StringComparison.OrdinalIgnoreCase))
                {
                    var roleAssignResult = await _userManager.AddToRoleAsync(user, "Manager");
                    if (!roleAssignResult.Succeeded)
                    {
                        return AuthResult.Failed("Not assign role to user");
                    }

                    await _profileRepository.SaveChangeAsync();
                }
                    return await GenerateTokenAsync(user);
            }
            catch (Exception ex)
            {
                return AuthResult.Failed("Register account failed!");
            }
        }

        private async Task<AuthResult> GenerateTokenAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new (ClaimTypes.Email, user.Email ?? ""),
                new ("userProfileId", user.UserProfileId.ToString()),
            };

            claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)));

            var accessToken = _jwtService.GenerateAccessToken(claims);
            var refreshToken = _jwtService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtConfig.RefreshTokenExpirationInDays);

            await _userManager.UpdateAsync(user);

            Patient patient = null;
            List<string> orderIds = new List<string>();
            if (roles.Any(x => x.Equals("User", StringComparison.OrdinalIgnoreCase)))
            {
                patient = await _patientRepository.FindByProfileIdAsync(user.UserProfileId);
                var orders = await _orderRepository.FindAllByPatientIdAsync(patient.Id);
                orderIds = orders.Select(x => x.Id.ToString()).ToList();
            }

            return AuthResult.Success(new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtConfig.ExpirationInMinutes),
                User = new UserDto
                {
                    Id = user.Id.ToString(),
                    ProfileId = user.UserProfileId.ToString(),
                    Email = user.Email,
                    PatientId = patient != null ? patient.Id.ToString() : "",
                    FirstName = user.UserProfile.FirstName,
                    MiddleName = user.UserProfile.MiddleName,
                    LastName = user.UserProfile.LastName,
                    AvatarUrl = user.UserProfile.AvatarUrl,
                    OrderIds = orderIds
                },
                Role = roles.First()
            });
        }
    }
}
