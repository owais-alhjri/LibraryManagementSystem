using System.Security.Claims;
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

        private Guid GetCurrentUserId()
        {
            var sub = User.FindFirstValue(ClaimTypes.NameIdentifier)
                      ?? throw new UnauthorizedAccessException("User ID not found in token");
            return Guid.Parse(sub);
        }

        [Authorize(Roles = "ADMIN,LIBRARIAN,MEMBER")]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult> FetchBorrowedBook([FromRoute] Guid id)
        {
            var borrowRecord = await _borrowRecordService.GetBorrowedRecordById(id);

            return Ok(borrowRecord);
        }

        [Authorize(Roles = "ADMIN,MEMBER")]
        [HttpPost]
        public async Task<ActionResult<Guid>> BorrowBook([FromBody] BorrowRecordCreateDto dto)
        {
            var userId = GetCurrentUserId();
            var borrowId = await borrowRecordService.BorrowBook(userId, dto.BookId);

            var borrowedInfo = new BorrowRecordResponseDto
            {
                Id = borrowId,
                UserId = userId,
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
            var userId = GetCurrentUserId();
            var result = await _borrowRecordService.ReturnBook(userId, dto.BookId);
            return Ok(new ReturnBookResponseDto
            {
                Id = result.Id,
                ReturnedDate = result.ReturnedDate,
                Message = "Book returned successfully",
            });
        }

        [Authorize]
        [HttpGet("my")]
        public async Task<ActionResult> GetMyBorrowRecord()
        {
            var userId = GetCurrentUserId();
            var records = await borrowRecordService.GetBorrowRecordsByUserId(userId);
            return Ok(records);
        }
    }
}
