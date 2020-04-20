using Microsoft.EntityFrameworkCore;

namespace UserService.Persistence
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
            :base(options)
        {
        }

        public DbSet<User> Users { get; set;}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().ToTable("User");
        }
    }
}