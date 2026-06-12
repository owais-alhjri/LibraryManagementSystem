
using LMS.Domain.Entities;

namespace LMS.Domain.Interfaces
{
    public interface IBookRepository
    {
        Task AddAsync(Book book);
        Task<List<Book>> GetAllAsync();

        Task SaveChangesAsync();

        Task<Book?> GetByIdAsync(Guid id);

        Task DeleteByIdAsync(Guid id);
    }
}
