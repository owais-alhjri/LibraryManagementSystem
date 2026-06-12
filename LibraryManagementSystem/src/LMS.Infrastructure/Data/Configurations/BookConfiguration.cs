

using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Persistence.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Books");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id)
                .ValueGeneratedNever();

            builder.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(b => b.Author)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(b => b.State)
                .IsRequired()
                .HasConversion<string>();

            builder.HasIndex(b => b.Title)
                .IsUnique();
        }
    }
}
