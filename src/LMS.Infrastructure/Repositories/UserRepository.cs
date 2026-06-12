using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly LmsDbContext _dbContext;

        public UserRepository(LmsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddUserAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
        public async Task<User?> GetByIdAsync(Guid id)
        {
            var userId = await _dbContext.Users.FindAsync(id);

            return userId;
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            var userEmail = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            return userEmail;
        }

    }
}
