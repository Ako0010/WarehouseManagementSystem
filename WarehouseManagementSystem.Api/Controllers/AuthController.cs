using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WarehouseManagementSystem.Application.DTOs;
using WarehouseManagementSystem.Application.Interface;
using WarehouseManagementSystem.Domain.Common;
using WarehouseManagementSystem.Domain.Model;

namespace WarehouseManagementSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly UserManager<User> _userManager;

    public AuthController(IAuthService userService,UserManager<User> userManager)
    {
        _authService = userService;
        _userManager = userManager;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterRequest registerRequest)
    {
        Console.WriteLine($"Email: '{registerRequest?.Email}'");

        var result = await _authService.CreateUserAsync(registerRequest);
        if (result == null)
        {
            return BadRequest("Registration failed.");
        }
        return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(result));
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequest loginRequest)
    {
        var result = await _authService.LoginUserAsync(loginRequest);
        if (result == null)
        {
            return Unauthorized("Invalid email or password.");
        }

        return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(result));
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Refresh([FromBody] RefreshTokenRequest refreshTokenRequest)
    {
        var result = await _authService.RefreshTokenAsync(refreshTokenRequest);
        return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(result, "Token refresh successfully"));
    }

    [HttpPost("revoke")]
    public async Task<ActionResult> Revoke([FromBody] RefreshTokenRequest refreshTokenRequest)
    {
        await _authService.RevokeRefreshTokenAsync(refreshTokenRequest);
        return Ok(ApiResponse<AuthResponseDto>.SuccessResponse("Token revoke successfully"));
    }

    [Authorize(Policy = "UserOnly")]
    [HttpPut("me/profile")]
    public async Task<ActionResult> UpdateMyProfile([FromBody] UpdateProfileRequest req)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

        var result = await _authService.ProfileUpdateAsync(userId, new Domain.Model.User
        {
            FirstName = req.FirstName,
            LastName = req.LastName,
            Address = req.Address,
            PhoneNumber = req.PhoneNumber
        });

        return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(result, "Profile changed successfully"));
    }


    [HttpPut("me/change-password")]
    public async Task<ActionResult> ChangeMyPassword([FromBody] ChangePasswordRequest req)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

        var result = await _authService.ChangePasswordAsync(userId, req.CurrentPassword, req.NewPassword);

        return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(result, "Password changed successfully."));
    }
}
