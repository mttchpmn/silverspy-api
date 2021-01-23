using Microsoft.EntityFrameworkCore;
using SilverSpy.Models;

namespace SilverSpy.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseNpgsql("Server=127.0.0.1;port=5432;user id=postgres;password=postgres;database=silverspy_local;pooling=true")
                .UseSnakeCaseNamingConvention();
    }
}