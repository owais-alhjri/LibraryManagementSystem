using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories
{
    public class BorrowRecordRepository : IBorrowRecordRepository
    {
        private readonly LmsDbContext _dbContext;
        public BorrowRecordRepository(LmsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
        public async Task BorrowBookAsync(BorrowRecord borrowRecord)
        {

            await _dbContext.BorrowRecords.AddAsync(borrowRecord);
            
        }
        public Task<BorrowRecord?> GetActiveBorrowAsync(Guid userId, Guid bookId)
        {
             return _dbContext.BorrowRecords.FirstOrDefaultAsync(br=>
             br.UserId == userId &&
             br.BookId == bookId &&
             br.ReturnedDate == null);
        }

        public async Task<BorrowRecord?> GetById(Guid id)
        {
            var borrowed = await _dbContext.BorrowRecords.FindAsync(id);
            return borrowed;
        }
    }
}
