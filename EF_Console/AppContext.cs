using Microsoft.EntityFrameworkCore;
using EF_Console.Entity;

namespace EF_Console
{
    public class AppContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }

        public AppContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=ASUS\SQLEXPRESS;Database=LibraryDB;" +
                "Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}
