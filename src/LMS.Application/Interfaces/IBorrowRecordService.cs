using LMS.Application.DTOs.BorrowRecords;

namespace LMS.Application.Interfaces
{
    public interface IBorrowRecordService
    {
        public Task<Guid> BorrowBook(Guid userId, Guid bookId);
        public Task<ReturnBookResponseDto> ReturnBook(Guid userId, Guid bookId);
        public Task<BorrowRecordResponseDto?> GetBorrowedRecordById(Guid id);
    }
}
