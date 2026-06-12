namespace LMS.Application.DTOs.BorrowRecords
{
    public class BorrowRecordCreateDto
    {
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }

    }
}
