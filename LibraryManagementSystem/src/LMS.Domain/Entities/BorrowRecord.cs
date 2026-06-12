using LMS.Domain.Enums;

namespace LMS.Domain.Entities
{
    public class BorrowRecord
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public Guid BookId { get; private set; }

        public DateTime BorrowedDate { get; private set; }
        public DateTime? ReturnedDate { get; private set; }

        private BorrowRecord() { }

        public BorrowRecord(User user, Book book)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNull(book);

            Id = Guid.NewGuid();
            UserId = user.Id;
            BookId = book.Id;
            BorrowedDate = DateTime.UtcNow;
            ReturnedDate = null;

        }
        public void Return(Book book)
        {
            ArgumentNullException.ThrowIfNull(book);
            if(book.Id != BookId)
            {
                throw new InvalidOperationException("This borrow record dous not belong to the given book.");
            }
            if (ReturnedDate is not null)
            {
                throw new InvalidOperationException("Book is already returned");
            }
            book.Return();
            ReturnedDate = DateTime.UtcNow;
        }

    }
}
