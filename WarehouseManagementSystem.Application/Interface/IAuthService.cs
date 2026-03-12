using Microsoft.AspNetCore.Identity.Data;
using WarehouseManagementSystem.Application.DTOs;
using WarehouseManagementSystem.Domain.Model;

namespace WarehouseManagementSystem.Application.Interface;

public interface IAuthService
{
    Task<AuthResponseDto> CreateUserAsync(DTOs.RegisterRequest registerRequest);
    Task<AuthResponseDto> LoginUserAsync(DTOs.LoginRequest loginRequest);
    Task<AuthResponseDto> ProfileUpdateAsync(string userId, User updatedUser);
    Task<AuthResponseDto> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
    Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);
    Task RevokeRefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);
}
