namespace LMS.Application.DTOs.Book
{
    public class ResponseOfAllTheBooks
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string BookState { get; set; } = null!;
    }
}
