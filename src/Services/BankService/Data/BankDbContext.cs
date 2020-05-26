using BankService.Models;
using Microsoft.EntityFrameworkCore;

namespace BankService.Data
{
    public class BankDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Customer>()
                .HasMany(c => c.ReceivedPayments)
                .WithOne(p => p.Receiver);

            builder.Entity<Customer>()
                .HasMany(c => c.SendPayments)
                .WithOne(p => p.Sender);
        }
    }
}