using Microsoft.EntityFrameworkCore;
using EF_Console.Entity;

namespace EF_Console.Configuration
{
    /// <summary>
    /// Контекст БД
    /// </summary>
    public class Context : DbContext
    {
        private string _connectionString;

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }

        public Context(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
