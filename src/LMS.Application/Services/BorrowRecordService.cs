using LMS.Application.Common.Exceptions;
using LMS.Application.DTOs.BorrowRecords;
using LMS.Application.Interfaces;
using LMS.Domain.Entities;
using LMS.Domain.Interfaces;

namespace LMS.Application.Services
{
    public class BorrowRecordService(IBorrowRecordRepository recordRepository, IBookRepository bookRepository, IUserRepository userRepository) : IBorrowRecordService
    {

        public async Task<Guid> BorrowBook(BorrowRecordCreateDto borrowRecordCreateDto)
        {
            var book = await bookRepository.GetByIdAsync(borrowRecordCreateDto.BookId);
            if (book is null)
            {
                throw new NotFoundException("Book", borrowRecordCreateDto.BookId);
            }
            var user = await userRepository.GetByIdAsync(borrowRecordCreateDto.UserId);
            if (user is null)
            {
                throw new NotFoundException("User", borrowRecordCreateDto.UserId);
            }

            var borrow = new BorrowRecord(user, book);
            book.Borrow();
            await recordRepository.BorrowBookAsync(borrow);
            await recordRepository.SaveChangesAsync();

            return borrow.Id;
        }

        public async Task<ReturnBookResponseDto> ReturnBook(ReturnBookDto dto)
        {
            var book = await bookRepository.GetByIdAsync(dto.BookId) ?? throw new NotFoundException("Book", dto.BookId);

            var borrowRecord = await recordRepository.GetActiveBorrowAsync(dto.UserId, dto.BookId);
            if (borrowRecord is null)
            {
                throw new NotFoundException("BorrowRecord", $"UserId={dto.UserId} ,BookId={dto.BookId}");
            }

            borrowRecord.Return(book);
            await recordRepository.SaveChangesAsync();

            return new ReturnBookResponseDto
            {
                Id = borrowRecord.Id,
                ReturnedDate = borrowRecord.ReturnedDate ?? throw new InvalidOperationException("Return date was not set.")
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
    }
}
