using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SilverSpy.Models;

namespace SilverSpy.DataAccess
{
    public class TransactionContext : DbContext
    {
        public TransactionContext(DbContextOptions<TransactionContext> options) : base(options)
        {}
        
        public DbSet<TransactionItem> TransactionItems { get; set; }
    }
}