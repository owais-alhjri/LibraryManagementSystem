using LMS.Application.Common.Exceptions;
using LMS.Application.DTOs.BorrowRecords;
using LMS.Application.Interfaces;
using LMS.Domain.Entities;
using LMS.Domain.Interfaces;

namespace LMS.Application.Services
{
    public class BorrowRecordService(IBorrowRecordRepository recordRepository, IBookRepository bookRepository, IUserRepository userRepository) : IBorrowRecordService
    {

        public async Task<Guid> BorrowBook(Guid userId, Guid bookId)
        {
            var book = await bookRepository.GetByIdAsync(bookId)
                       ?? throw new NotFoundException("Book", bookId);

            var user = await userRepository.GetByIdAsync(userId)
                       ?? throw new NotFoundException("User", userId);

            var borrow = new BorrowRecord(user, book);
            book.Borrow();
            await recordRepository.BorrowBookAsync(borrow);
            await recordRepository.SaveChangesAsync();

            return borrow.Id;
        }

        public async Task<ReturnBookResponseDto> ReturnBook(Guid userId, Guid bookId)
        {
            var book = await bookRepository.GetByIdAsync(bookId)
                       ?? throw new NotFoundException("Book", bookId);

            var borrowRecord = await recordRepository.GetActiveBorrowAsync(userId, bookId)
                               ?? throw new NotFoundException("BorrowRecord", $"UserId={userId}, BookId={bookId}");

            borrowRecord.Return(book);
            await recordRepository.SaveChangesAsync();

            return new ReturnBookResponseDto
            {
                Id = borrowRecord.Id,
                ReturnedDate = borrowRecord.ReturnedDate
                               ?? throw new InvalidOperationException("Return date was not set.")
            };
        }

        public async Task<BorrowRecordResponseDto?> GetBorrowedRecordById(Guid id)
        {
            var borrowed = await recordRepository.GetById(id) ?? throw new NotFoundException("Borrowed Record", id);

            return new BorrowRecordResponseDto
            {
                Id = id,
                UserId = borrowed.UserId,
                BookId = borrowed.BookId,
                Message = "Borrowed Record Information",
                BorrowedDate = borrowed.BorrowedDate,
                ReturnedDate = borrowed.ReturnedDate
            };
        }

        public async Task<List<BorrowRecordResponseDto>> GetBorrowRecordsByUserId(Guid userId)
        {
            var records = await recordRepository.GetByUserIdAsync(userId);
            return records.Select(r => new BorrowRecordResponseDto
            {
                Id = r.Id,
                UserId = r.UserId,
                BookId = r.BookId,
                BookTitle = r.Book.Title,
                BorrowedDate = r.BorrowedDate,
                ReturnedDate = r.ReturnedDate,
                Message = "Borrow record"
            }).ToList();
        }
    }
}
