using EF_Console.Entity;
using EF_Console.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EF_Console.Repository
{
    /// <summary>
    /// Репозиторий жанра
    /// </summary>
    public class GenreRepository : IGenreRepository
    {
        private string _connect;

        public GenreRepository(string connect)
        {
            _connect = connect;
        }

        public Genre? FindByName(string name)
        {
            using(var db = new Context(_connect))
            {
                return db.Genres.AsNoTracking().FirstOrDefault(g => g.Genre_name == name);
            }
        }
    }

    /// <summary>
    /// Интерфейс репозитория жанра
    /// </summary>
    interface IGenreRepository
    {
        /// <summary>
        /// Получение жанра по названию
        /// </summary>
        Genre? FindByName(string name);
    }
}
