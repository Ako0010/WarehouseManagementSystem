using Microsoft.AspNetCore.Identity.Data;
using WarehouseManagementSystem.DTOs;
using WarehouseManagementSystem.Model;

namespace WarehouseManagementSystem.Service.Interface;

public interface IAuthService
{
    Task<AuthResponseDto> CreateUserAsync(DTOs.RegisterRequest registerRequest);
    Task<AuthResponseDto> LoginUserAsync(DTOs.LoginRequest loginRequest);
    Task<AuthResponseDto> ProfileUpdateAsync(string userId, User updatedUser);
    Task<AuthResponseDto> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
    Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);
    Task RevokeRefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);
}
