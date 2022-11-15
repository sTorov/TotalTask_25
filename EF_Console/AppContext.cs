using Microsoft.EntityFrameworkCore;
using EF_Console.Entity;

namespace EF_Console
{
    public class AppContext : DbContext
    {
        private static bool createDB;

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }

        static AppContext()
        {
            createDB = true;
        }

        public AppContext()
        {
            if(createDB)
            {
                createDB = false;
                Database.EnsureDeleted();
                Database.EnsureCreated();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=ASUS\SQLEXPRESS;Database=LibraryDB;" +
                "Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}
