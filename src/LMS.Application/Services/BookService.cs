using LMS.Application.Common.Exceptions;
using LMS.Application.DTOs.Book;
using LMS.Application.Interfaces;
using LMS.Domain.Entities;
using LMS.Domain.Enums;
using LMS.Domain.Interfaces;

namespace LMS.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<List<ResponseOfAllTheBooks>> GetAllBooksAsync()
        {
            var books = await _bookRepository.GetAllAsync();

            return books.Select(b => new ResponseOfAllTheBooks
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                BookState = b.State.ToString()
            }).ToList();
        }

        public async Task<Book> AddBookAsync(CreateBookDto createBookDto)
        {
            var book = new Book(createBookDto.Title, createBookDto.Author);

            await _bookRepository.AddAsync(book);
            await _bookRepository.SaveChangesAsync();

            return book;
        }

        public async Task<Book> UpdateBookAsync(UpdateBookPatchDto updateBook, Guid id)
        {
            var book = await _bookRepository.GetByIdAsync(id) ?? throw new NotFoundException("Book", id);

            BookState? parsedState = null;

            if (updateBook.BookState is not null)
            {
                if (!Enum.TryParse<BookState>(updateBook.BookState, true, out var state))
                {

                    throw new ValidationException("Invalid BookState");

                }

                parsedState = state;
            }
            if(updateBook.Title is null && updateBook.Author is null && updateBook.BookState is null)
            {
                throw new ValidationException("At least one field mus be provided for update.");
            }
            book.UpdateBook(updateBook.Title, updateBook.Author, parsedState);
            await _bookRepository.SaveChangesAsync();

            return book;

        }

        public async Task<Book> DeleteBook(Guid id)
        {
            var book = await _bookRepository.GetByIdAsync(id) ?? throw new NotFoundException("Book", id);

            await _bookRepository.DeleteByIdAsync(id);
            await _bookRepository.SaveChangesAsync();
            return book;
        }
        public async Task<ResponseBookDto?> GetBookByIdAsync(Guid id)
        {
            var book = await _bookRepository.GetByIdAsync(id) ?? throw new NotFoundException("Book", id);


            return new ResponseBookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                BookState = book.State.ToString(),
                Message = "Book info"
            };
        }

    }
}
