using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WarehouseManagementSystem.Config;
using WarehouseManagementSystem.Infrastructure.Data;
using WarehouseManagementSystem.Application.DTOs;
using WarehouseManagementSystem.Domain.Model;
using WarehouseManagementSystem.Application.Interface;

namespace WarehouseManagementSystem.Infrastructure.Service;

public class AuthService : IAuthService
{
    private const string RefreshTokenType = "refresh";
    private readonly UserManager<User> _userManager;
    private readonly WarehouseManagementDBContext _context;
    private readonly JwtConfig _config;

    public AuthService(
        UserManager<User> userManager,
        WarehouseManagementDBContext context,
        IOptions<JwtConfig> config)
    {
        _userManager = userManager;
        _context = context;
        _config = config.Value;
    }

    public async Task<AuthResponseDto> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new KeyNotFoundException("User not found.");

        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));

        return await GenerateTokensAsync(user);
    }

    public async Task<AuthResponseDto> CreateUserAsync(RegisterRequest registerRequest)
    {
        if (string.IsNullOrWhiteSpace(registerRequest.Email))
            throw new ArgumentException("Email is required.");

        var existingUser = await _userManager.FindByEmailAsync(registerRequest.Email);
        if (existingUser != null)
            throw new InvalidOperationException("User with this email already exists.");

        var user = new User
        {
            UserName = registerRequest.Email,
            Email = registerRequest.Email,
            FirstName = registerRequest.FirstName,
            LastName = registerRequest.LastName,
            Address = registerRequest.Address,
            PhoneNumber = registerRequest.PhoneNumber,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = null
        };



        var result = await _userManager.CreateAsync(user, registerRequest.Password);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                Console.WriteLine(error.Description);
            }
            throw new InvalidOperationException("User creation failed.");
        }

        await _userManager.AddToRoleAsync(user, "User");

        return await GenerateTokensAsync(user);
    }

    public async Task<AuthResponseDto> LoginUserAsync(LoginRequest loginRequest)
    {
        var user = await _userManager.FindByEmailAsync(loginRequest.Email);
        if (user == null)
            throw new UnauthorizedAccessException("Invalid email or password.");

        if (!await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            throw new UnauthorizedAccessException("Invalid email or password.");

        user.IsOnline = true;
        user.LastSeen = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        return await GenerateTokensAsync(user);
    }

    public async Task<AuthResponseDto> ProfileUpdateAsync(string userId, User updatedUser)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new KeyNotFoundException("User not found.");

        if (!string.IsNullOrWhiteSpace(updatedUser.FirstName))
            user.FirstName = updatedUser.FirstName;

        if (!string.IsNullOrWhiteSpace(updatedUser.LastName))
            user.LastName = updatedUser.LastName;

        if (!string.IsNullOrWhiteSpace(updatedUser.PhoneNumber))
            user.PhoneNumber = updatedUser.PhoneNumber;

        if (!string.IsNullOrWhiteSpace(updatedUser.Address))
            user.Address = updatedUser.Address;

        user.UpdatedAt = DateTimeOffset.UtcNow;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));

        return await GenerateTokensAsync(user);
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var (principal, jti) = ValidateRefreshJwtAndGetJti(request.RefreshToken);

        var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.JwtId == jti);
        if (storedToken == null || !storedToken.IsActive)
            throw new UnauthorizedAccessException("Invalid refresh token.");

        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier) ?? storedToken.UserId;
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new UnauthorizedAccessException("User not found.");

        storedToken.RevokedAt = DateTimeOffset.UtcNow;

        var newTokens = await GenerateTokensAsync(user);

        var newStored = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.JwtId == GetJtiFromRefreshToken(newTokens.RefreshToken));

        if (newStored != null)
            storedToken.ReplacedByJwtId = newStored.JwtId;

        await _context.SaveChangesAsync();
        return newTokens;
    }

    public async Task RevokeRefreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
    {
        string? jti;
        try
        {
            (_, jti) = ValidateRefreshJwtAndGetJti(refreshTokenRequest.RefreshToken, false);
        }
        catch
        {
            return;
        }

        var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.JwtId == jti);
        if (storedToken == null || !storedToken.IsActive)
            return;

        storedToken.RevokedAt = DateTimeOffset.UtcNow;
        await _context.SaveChangesAsync();
    }

    private async Task<AuthResponseDto> GenerateTokensAsync(User user)
    {
        var accessToken = await GenerateAccessTokenAsync(user, _config.ExpirationMinutes);
        var (refreshEntity, refreshJwt) = await CreateRefreshTokenJwtAsync(user.Id, _config.RefreshTokenExpirationDays);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            ExpiresAt = DateTimeOffset.UtcNow.AddMinutes(_config.ExpirationMinutes),
            RefreshToken = refreshJwt,
            RefreshTokenExpiresAt = refreshEntity.ExpiresAt,
            Email = user.Email ?? string.Empty
        };
    }

    private async Task<(RefreshToken entity, string jwt)> CreateRefreshTokenJwtAsync(string userId, int expirationDays)
    {
        var jti = Guid.NewGuid().ToString();
        var expiresAt = DateTimeOffset.UtcNow.AddDays(expirationDays);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.RefreshTokenSecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(JwtRegisteredClaimNames.Jti, jti),
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim("token_type", RefreshTokenType)
        };

        var token = new JwtSecurityToken(
            issuer: _config.Issuer,
            audience: _config.Audience,
            claims: claims,
            expires: expiresAt.UtcDateTime,
            signingCredentials: credentials
        );

        var jwtString = new JwtSecurityTokenHandler().WriteToken(token);

        var entity = new RefreshToken
        {
            JwtId = jti,
            UserId = userId,
            ExpiresAt = expiresAt,
            CreatedAt = DateTimeOffset.UtcNow
        };

        _context.RefreshTokens.Add(entity);
        await _context.SaveChangesAsync();

        return (entity, jwtString);
    }

    private async Task<string> GenerateAccessTokenAsync(User user, int expirationMinutes)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var token = new JwtSecurityToken(
            issuer: _config.Issuer,
            audience: _config.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private (ClaimsPrincipal principal, string jti) ValidateRefreshJwtAndGetJti(string refreshToken, bool validateLifetime = true)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.RefreshTokenSecretKey));

        var principal = handler.ValidateToken(refreshToken, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = true,
            ValidIssuer = _config.Issuer,
            ValidateAudience = true,
            ValidAudience = _config.Audience,
            ValidateLifetime = validateLifetime,
            ClockSkew = TimeSpan.Zero
        }, out var validatedToken);

        if (validatedToken is not JwtSecurityToken jwt)
            throw new UnauthorizedAccessException("Invalid refresh token");

        var tokenType = jwt.Claims.FirstOrDefault(c => c.Type == "token_type")?.Value;
        if (tokenType != RefreshTokenType)
            throw new UnauthorizedAccessException();

        var jti = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value
            ?? throw new UnauthorizedAccessException();

        return (principal, jti);
    }

    private static string GetJtiFromRefreshToken(string refreshJwt)
    {
        var handler = new JwtSecurityTokenHandler();
        if (!handler.CanReadToken(refreshJwt))
            return string.Empty;

        var jwt = handler.ReadJwtToken(refreshJwt);
        return jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value ?? string.Empty;
    }
}
