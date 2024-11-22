using Microsoft.EntityFrameworkCore;
using MyCalculatorApi.Models;

namespace MyCalculatorApi.Data
{
    public class CalculatorContext : DbContext
    {
        public DbSet<CalculatorStep> Steps { get; set; }

        public CalculatorContext(DbContextOptions<CalculatorContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=calculator.db");
            }
        }
    }
}