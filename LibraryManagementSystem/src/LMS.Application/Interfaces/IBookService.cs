using LMS.Application.DTOs.Book;
using LMS.Domain.Entities;

namespace LMS.Application.Interfaces
{
    public interface IBookService
    {
        Task<List<ResponseOfAllTheBooks>> GetAllBooksAsync();

        Task<Book> AddBookAsync(CreateBookDto createBookDto);
        Task<Book> UpdateBookAsync(UpdateBookPatchDto updateBook, Guid id);
        Task<Book> DeleteBook(Guid id);
        Task<ResponseBookDto?> GetBookByIdAsync(Guid id);
    }
}
