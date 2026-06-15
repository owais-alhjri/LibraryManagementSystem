using LMS.Application.DTOs.Book;
using LMS.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController(IBookService bookService) : ControllerBase
    {

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetAllBooks(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null)
        {
            var result = await bookService.GetAllBooksAsync(page, pageSize, search);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult> GetBookById([FromRoute] Guid id) => Ok(await bookService.GetBookByIdAsync(id));

        [Authorize(Roles = "ADMIN,LIBRARIAN")]
        [HttpPost]
        public async Task<ActionResult> AddBook([FromBody] CreateBookDto createBookDto)
        {
            var book = await bookService.AddBookAsync(createBookDto);
            return Ok(new ResponseBookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                BookState = book.State.ToString(),
                Message = "Book is registered successfully"
            });
        }

        [Authorize(Roles = "ADMIN,LIBRARIAN")]
        [HttpPatch("{id:guid}")]
        public async Task<ActionResult> UpdateBook([FromBody] UpdateBookPatchDto updateBookDto, [FromRoute] Guid id)
        {
            var book = await bookService.UpdateBookAsync(updateBookDto, id);
            return Ok(new ResponseBookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                BookState = book.State.ToString(),
                Message = "Book updated successfully"
            });
        }


        [Authorize(Roles = "ADMIN,LIBRARIAN")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteBook([FromRoute] Guid id)
        {
            var book = await bookService.DeleteBook(id);

            return Ok(new DeleteBookResponseDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Message = "Book is deleted successfully"
            });
        }


    }
}
