using Microsoft.EntityFrameworkCore;
using Sdf2.Database.CodeFirst.Configurations;
using Sdf2.Database.CodeFirst.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Sdf2.Database.CodeFirst.Context
{
    public class AppDbContext : DbContext
    {
        private readonly string connectionString = "Server=USER\\SQLEXPRESS;Database=SdfTwo;Trusted_Connection=True;TrustServerCertificate=True";

        public DbSet<User> Users { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseType> ExpenseTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ExpenseTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ExpenseConfiguration());
        }
    }
}
