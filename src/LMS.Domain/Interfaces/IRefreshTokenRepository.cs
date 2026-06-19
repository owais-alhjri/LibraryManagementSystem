using LMS.Domain.Entities;

namespace LMS.Domain.Interfaces;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken token);
    Task<RefreshToken> GetByTokenAsync(string token);
    Task SaveChangesAsync();
}