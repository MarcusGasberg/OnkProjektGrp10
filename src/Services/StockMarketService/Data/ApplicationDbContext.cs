using Microsoft.EntityFrameworkCore;
using StockMarketService.Models;

namespace StockMarketService
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Stock> Stocks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=blogging.db");
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StockPrice>()
                .HasOne<Stock>(s => s.stock)
                .WithMany(s => s.HistoricPrice)
                .IsRequired();

            modelBuilder.Entity<Stock>()
                .Property(s => s.Id)
                .ValueGeneratedOnAdd();
            
            modelBuilder.Entity<StockPrice>()
                .Property(s => s.Id)
                .ValueGeneratedOnAdd();
            
            modelBuilder.Entity<Seller>()
                .Property(s => s.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Seller>()
                .HasOne<Stock>(s => s.Stock)
                .WithMany(s => s.Seller)
                .IsRequired();
        }
        
    }
}