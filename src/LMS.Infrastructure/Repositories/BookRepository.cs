using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories
{
    public class BookRepository :IBookRepository
    {
        private readonly LmsDbContext _dbContext;

        public BookRepository(LmsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Book book)
        {
            await _dbContext.Books.AddAsync(book);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Book>> GetAllAsync()
        {
            return await _dbContext.Books.AsNoTracking().ToListAsync();
        }  

        public async Task<Book?> GetByIdAsync(Guid id )
        {
             var book = await _dbContext.Books.FindAsync(id);
            return book;
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var book = await _dbContext.Books.FindAsync(id);
            if (book == null)
            {
                return;
            }
            _dbContext.Remove(book);
        }

    }
}
