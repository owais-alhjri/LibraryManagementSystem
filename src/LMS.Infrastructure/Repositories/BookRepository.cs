using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories
{
    public class BookRepository(LmsDbContext dbContext) : IBookRepository
    {
        public async Task AddAsync(Book book)
        {
            await dbContext.Books.AddAsync(book);
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public async Task<Book?> GetByIdAsync(Guid id )
        {
             var book = await dbContext.Books.FindAsync(id);
            return book;
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var book = await dbContext.Books.FindAsync(id);
            if (book == null)
            {
                return;
            }
            dbContext.Remove(book);
        }

        public async Task<(List<Book> Items, int TotalCount)> GetAllAsync(int page, int pageSize, string? search)
        {
            var query = dbContext.Books.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = $"%{search.Trim()}%";
                query = query.Where(b =>
                    EF.Functions.ILike(b.Title, term) ||
                    EF.Functions.ILike(b.Author, term));
            }

            var totalCount = await query.CountAsync();

            var items = await 
                query.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);

        }

    }
}
