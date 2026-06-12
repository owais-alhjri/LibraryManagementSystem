
using LMS.Domain.Enums;

namespace LMS.Domain.Entities
{
    public class Book
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Author { get; private set; }

        public BookState State { get; private set; } = BookState.Available;

        private Book() { }

        public Book(string title, string author)
        {
            title = title.Trim();
            author = author.Trim();

            ValidateTitle(title);
            ValidateAuthor(author);

            Id = Guid.NewGuid();
            Title = title;
            Author = author;
        }

        private static void ValidateTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title cannot be empty");
            }

            if (title.Length < 3 || title.Length > 50)
            {
                throw new ArgumentException("Title must be in range of 3 to 50 characters");
            }
        }

        private static void ValidateAuthor(string author)
        {
            if (string.IsNullOrWhiteSpace(author))
            {
                throw new ArgumentException("Author cannot be empty");
            }

            if (author.Length < 3 || author.Length > 50)
            {
                throw new ArgumentException("Author must be in range of 3 to 50 characters");
            }
        }

        public void Borrow()
        {
            Console.WriteLine($"Before check: {State}");

            if (State == BookState.Borrowed)
            {
                Console.WriteLine("IF BLOCK EXECUTED");
                throw new InvalidOperationException("Book is already borrowed");
            }

            Console.WriteLine("Borrowing book now...");
            State = BookState.Borrowed;
        }


        public void Return()
        {
            if (State == BookState.Available)
                throw new InvalidOperationException("Book is not borrowed");

            State = BookState.Available;
        }


        public void UpdateBook(string? title, string? author, BookState? state)
        {
            if (title is not null)
            {
                ValidateTitle(title);
                Title = title.Trim();
            }

            if (author is not null)
            {
                ValidateAuthor(author);
                Author = author.Trim();
            }
            if (state.HasValue)
            {
                State = state.Value;
            }


        }

    }

}
