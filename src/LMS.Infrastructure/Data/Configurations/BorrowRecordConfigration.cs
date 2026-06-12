using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Persistence.Configurations
{
    public class BorrowRecordConfigration : IEntityTypeConfiguration<BorrowRecord>
    {
        public void Configure(EntityTypeBuilder<BorrowRecord> builder)
        {
            builder.ToTable("BorrowRecords");
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id)
                .ValueGeneratedNever();
            builder.Property(b => b.UserId)
                .IsRequired();
            builder.Property(b => b.BookId)
                .IsRequired();
            builder.Property(b => b.BorrowedDate)
                .IsRequired();
            builder.Property(b => b.ReturnedDate);

                
        }
    }
}
