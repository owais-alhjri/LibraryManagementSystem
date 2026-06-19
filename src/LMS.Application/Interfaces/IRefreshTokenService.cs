using LMS.Domain.Entities;

namespace LMS.Application.Interfaces;

public interface IRefreshTokenService
{
    Task<RefreshToken> GenerateAndStoreAsync(Guid userId);
    Task<RefreshToken?> ValidateAsync(string token);
    Task<RefreshToken> RotateAsync(RefreshToken oldToken);
    Task RevokeAsync(RefreshToken token);
}