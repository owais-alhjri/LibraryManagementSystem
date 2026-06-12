using LMS.Domain.Entities;

namespace LMS.Domain.Interfaces
{
    public interface IBorrowRecordRepository
    {
        Task BorrowBookAsync(BorrowRecord borrowRecord);
        Task SaveChangesAsync();
        Task<BorrowRecord?> GetActiveBorrowAsync(Guid userId, Guid bookId);

        Task<BorrowRecord?> GetById(Guid id);
    }
}
