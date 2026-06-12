using LMS.Domain.Entities;

namespace LMS.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);

        Task SaveChangesAsync();
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
    }
}
