﻿using FertilityCare.Infrastructure.Services;
using FertilityCare.UseCase.DTOs.Auths;
using FertilityCare.WebAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FertilityCare.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;

        private readonly IAccountService _accountService;

        public AuthController(IAuthService authService, IAccountService accounttService)
        {
            _authService = authService;
            _accountService = accounttService;
        }

        [HttpGet("accounts")]
        public async Task<ActionResult<ApiResponse<AccountSideAdmin>>> GetAccountSideAdmins()
        {
            try
            {
                var accounts = await _accountService.GetAccountSideAdmins();
                return Ok(new ApiResponse<IEnumerable<AccountSideAdmin>>
                {
                    StatusCode = 200,
                    Message = "Accounts fetched successfully",
                    Data = accounts,
                    ResponsedAt = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = 400,
                    Message = ex.Message,
                    Data = null,
                    ResponsedAt = DateTime.Now
                });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResult>> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.LoginAsync(request);
            if (!result.IsSuccess)
            {
                return BadRequest(new { messsage = result.ErrorMessage });
            }

            return Ok(result.Data);
        }

        [HttpPost("google-login")]
        public async Task<ActionResult<AuthResult>> LoginGoogleAuth([FromBody] GoogleLoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.GoogleLoginAsync(request);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.ErrorMessage });

            return Ok(result.Data);
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResult>> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(request);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.ErrorMessage });

            return Ok(result.Data);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResult>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RefreshTokenAsync(request);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.ErrorMessage });

            return Ok(result.Data);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Invalid user");
            }

            var success = await _authService.LogoutAsync(userId);

            if (!success)
            {
                return BadRequest("Logout failed");
            }

            return Ok(new { message = "Logged out successfully" });
        }

        [Authorize(Roles = "User")]
        [HttpPost("me")]
        public IActionResult GetCurrentUser()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

            return Ok(new { userId, email });
        }
    }
}
