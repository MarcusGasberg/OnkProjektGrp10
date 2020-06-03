using Microsoft.EntityFrameworkCore;
using StockMarketService.Models;

namespace StockMarketService
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Stock> Stocks { get; set; }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StockPrice>()
                .HasOne<Stock>(s => s.stock)
                .WithMany(s => s.HistoricPrice);

            modelBuilder.Entity<Stock>();

            modelBuilder.Entity<Seller>()
                .HasOne<Stock>(s => s.Stock)
                .WithMany(s => s.Seller);
        }
        
    }
}