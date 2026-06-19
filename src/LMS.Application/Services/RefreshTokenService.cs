using System.Security.Cryptography;
using LMS.Application.Interfaces;
using LMS.Application.Settings;
using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace LMS.Application.Services;

public class RefreshTokenService(IRefreshTokenRepository refreshTokenRepository, IOptions<JwtSettings> jwtSettings) : IRefreshTokenService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public async Task<RefreshToken> GenerateAndStoreAsync(Guid userId)
    {
        var tokenValue = GenerateSecureToken();
        var refreshToken = new RefreshToken(tokenValue, userId,
            DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays));
        await refreshTokenRepository.AddAsync(refreshToken);
        await refreshTokenRepository.SaveChangesAsync();
        return refreshToken;
    }
    public async Task<RefreshToken?> ValidateAsync(string token)
    {
        var refreshToken = await refreshTokenRepository.GetByTokenAsync(token);
        return refreshToken is { IsActive: true } ? refreshToken : null;
    }
    public async Task<RefreshToken> RotateAsync(RefreshToken oldToken)
    {
        var newToken = await GenerateAndStoreAsync(oldToken.UserId);
        oldToken.Revoke(newToken.Token);
        await refreshTokenRepository.SaveChangesAsync();
        return newToken;
    }

    public async Task RevokeAsync(RefreshToken token)
    {
        token.Revoke();
        await refreshTokenRepository.SaveChangesAsync();
    }
    private static string GenerateSecureToken()
    {
        var bytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
}