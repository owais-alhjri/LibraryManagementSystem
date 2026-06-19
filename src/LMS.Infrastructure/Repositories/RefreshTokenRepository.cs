using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories;

public class RefreshTokenRepository(LmsDbContext dbContext) : IRefreshTokenRepository
{
    public async Task AddAsync(RefreshToken token)
    {
        await dbContext.RefreshTokens.AddAsync(token);
    }

    public Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return dbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token);
    }

    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}