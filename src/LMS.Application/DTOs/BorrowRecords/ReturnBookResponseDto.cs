namespace LMS.Application.DTOs.BorrowRecords
{
    public class ReturnBookResponseDto
    {
        public Guid Id { get; init; }
        public DateTime ReturnedDate { get; init; }
        public string Message { get; set; } = string.Empty;
    }
}
