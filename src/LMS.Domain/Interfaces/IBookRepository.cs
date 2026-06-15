
using LMS.Domain.Entities;

namespace LMS.Domain.Interfaces
{
    public interface IBookRepository
    {
        Task AddAsync(Book book);

        Task SaveChangesAsync();

        Task<Book?> GetByIdAsync(Guid id);

        Task DeleteByIdAsync(Guid id);

        Task<(List<Book> Items, int TotalCount)> GetAllAsync(int page, int pageSize, string? search);


    }
}
