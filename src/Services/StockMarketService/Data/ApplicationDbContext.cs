using Microsoft.EntityFrameworkCore;
using StockMarketService.Models;

namespace StockMarketService
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Stock> Stocks { get; set; }

        /*protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Server=stock-db, 1433;Database=StockDb;User=sa;Password=Passw0rd");
        }*/
        
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