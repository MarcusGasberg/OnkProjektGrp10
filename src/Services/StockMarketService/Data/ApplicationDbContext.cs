using Microsoft.EntityFrameworkCore;

namespace StockMarketService
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Stock> stocks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=blogging.db");
    }
}