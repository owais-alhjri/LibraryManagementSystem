using LMS.Domain.Entities;
using LMS.Domain.Enums;
using LMS.Domain.Interfaces;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories
{
    public class UserRepository(LmsDbContext dbContext) : IUserRepository
    {
        public async Task AddUserAsync(User user)
        {
            await dbContext.Users.AddAsync(user);
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
        public async Task<User?> GetByIdAsync(Guid id)
        {
            var userId = await dbContext.Users.FindAsync(id);

            return userId;
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            var userEmail = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            return userEmail;
        }

        public async Task UpdateAsync(User user)
        {
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllUsersExceptAdminAsync()
        {
            var users = await dbContext.Users
                .Where(u => u.Role != Roles.ADMIN)
                .AsNoTracking()
                .ToListAsync();
            return users;
        }

    }
}
