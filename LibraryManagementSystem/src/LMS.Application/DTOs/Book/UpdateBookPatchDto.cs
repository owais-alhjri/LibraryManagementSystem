namespace LMS.Application.DTOs.Book
{
    public class UpdateBookPatchDto
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? BookState { get; set; }
    }
}
