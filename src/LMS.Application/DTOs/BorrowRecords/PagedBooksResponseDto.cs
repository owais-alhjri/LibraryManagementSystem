namespace LMS.Application.DTOs.BorrowRecords;

public class PagedBooksResponseDto
{
    public List<Domain.Entities.Book> Items { get; set; } = new();

    public int TotalCount { get; set; }

    public int Page { get; set; }

    public int TotalPages { get; set; }
}