using LMS.Application.DTOs.BorrowRecords;

namespace LMS.Application.Interfaces
{
    public interface IBorrowRecordService
    {
        public Task<Guid> BorrowBook(BorrowRecordCreateDto borrowRecordCreateDto);
        public Task<ReturnBookResponseDto> ReturnBook(ReturnBookDto returnBookDto);
        public Task<BorrowRecordResponseDto?> GetBorrowedaRecordById(Guid id);
    }
}
