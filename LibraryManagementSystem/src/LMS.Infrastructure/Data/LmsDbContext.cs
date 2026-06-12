using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Data
{
    public class LmsDbContext : DbContext
    {
        public LmsDbContext(DbContextOptions<LmsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<BorrowRecord> BorrowRecords { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LmsDbContext).Assembly);
        }

    }
}
