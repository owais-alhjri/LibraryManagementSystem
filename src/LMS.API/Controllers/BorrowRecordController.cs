using LMS.Application.DTOs.BorrowRecords;
using LMS.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers
{
    [Route("api/borrow-records")]
    [ApiController]
    public class BorrowRecordController(IBorrowRecordService borrowRecordService) : ControllerBase
    {
        private readonly IBorrowRecordService _borrowRecordService = borrowRecordService;

        [Authorize(Roles = "ADMIN,LIBRARIAN,MEMBER")]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult> FetchBorrowedBook([FromRoute] Guid id)
        {
            var borooed = await _borrowRecordService.GetBorrowedaRecordById(id);

            return Ok(borooed);
        }

        [Authorize(Roles = "ADMIN,MEMBER")]
        [HttpPost]
        public async Task<ActionResult<Guid>> BorrowBook([FromBody] BorrowRecordCreateDto dto)
        {
            var borrowId = await _borrowRecordService.BorrowBook(dto);
            var borrowedInfo = new BorrowRecordResponseDto
            {
                Id = borrowId,
                UserId = dto.UserId,
                BookId = dto.BookId,
                Message = "You have borrowed successfully",
                BorrowedDate = DateTime.UtcNow

            };
            return CreatedAtAction(nameof(FetchBorrowedBook), new { id = borrowId }, borrowedInfo);
        }

        [Authorize(Roles = "ADMIN,MEMBER")]
        [HttpPost("return")]
        public async Task<ActionResult<ReturnBookResponseDto>> ReturnBook([FromBody] ReturnBookDto dto)
        {
            var result = await _borrowRecordService.ReturnBook(dto);
            return Ok(new ReturnBookResponseDto
            {
                Id = result.Id,
                ReturnedDate = result.ReturnedDate,
                Message = "Book returned successfully",
            });
        }
    }
}
