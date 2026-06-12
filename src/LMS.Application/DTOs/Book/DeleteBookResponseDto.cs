namespace LMS.Application.DTOs.Book
{
    public class DeleteBookResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string Message { get; set; } = string.Empty;
    }
}
