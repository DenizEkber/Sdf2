using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Sdf2.Database.CodeFirst.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sdf2.Database.CodeFirst.Configurations
{
    public class ExpenseTypeConfiguration : IEntityTypeConfiguration<ExpenseType>
    {
        public void Configure(EntityTypeBuilder<ExpenseType> builder)
        {
            builder.HasKey(et => et.Id);

            builder.Property(et => et.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasMany(et => et.Expenses)
                .WithOne(e => e.ExpenseType)
                .HasForeignKey(e => e.ExpenseTypeId);
        }
    }
}
