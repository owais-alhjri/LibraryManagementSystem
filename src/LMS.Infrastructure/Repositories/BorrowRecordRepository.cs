using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories
{
    public class BorrowRecordRepository(LmsDbContext dbContext) : IBorrowRecordRepository
    {
        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
        public async Task BorrowBookAsync(BorrowRecord borrowRecord)
        {

            await dbContext.BorrowRecords.AddAsync(borrowRecord);
            
        }
        public Task<BorrowRecord?> GetActiveBorrowAsync(Guid userId, Guid bookId)
        {
             return dbContext.BorrowRecords.FirstOrDefaultAsync(br=>
             br.UserId == userId &&
             br.BookId == bookId &&
             br.ReturnedDate == null);
        }

        public async Task<BorrowRecord?> GetById(Guid id)
        {
            var borrowed = await dbContext.BorrowRecords.FindAsync(id);
            return borrowed;
        }

        public async Task<List<BorrowRecord>> GetByUserIdAsync(Guid userId)
        {
            return await dbContext.BorrowRecords
                .AsNoTracking()
                .IgnoreQueryFilters()
                .Include(r => r.Book)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.BorrowedDate)
                .ToListAsync();
        }
    }
}
