using FertilityCare.Domain.Entities;
using FertilityCare.Infrastructure.Configurations;
using FertilityCare.Infrastructure.Identity;
using FertilityCare.UseCase.DTOs.Auths;
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

        private readonly IJwtService _jwtService;

        private readonly JwtConfiguration _jwtConfig;

        private readonly GoogleAuthConfiguration _googleConfig;

        public AuthService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtService jwtService, IOptions<JwtConfiguration> jwtConfig,
            IOptions<GoogleAuthConfiguration> googleConfig)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _jwtConfig = jwtConfig.Value;
            _googleConfig = googleConfig.Value;
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
                            AvatarUrl = payload.Picture
                        }
                    };

                    var createResult = await _userManager.CreateAsync(newUser);
                    if (!createResult.Succeeded)
                        return AuthResult.Failed("Tạo tài khoản thất bại");

                    var roleResult = await _userManager.AddToRoleAsync(newUser, "User");
                    if (!roleResult.Succeeded)
                        return AuthResult.Failed("Không thể gán vai trò cho người dùng");

                    newUser.LastLogin = DateTime.Now;
                    await _userManager.UpdateAsync(newUser);

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

        public Task<AuthResult> RegisterAsync(RegisterRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
