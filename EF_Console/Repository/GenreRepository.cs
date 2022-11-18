using EF_Console.Entity;
using EF_Console.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EF_Console.Repository
{
    /// <summary>
    /// Репозиторий жанра
    /// </summary>
    public class GenreRepository : IGenreRepository, IRepository<Genre>
    {
        private string _connect;

        public GenreRepository(string connect)
        {
            _connect = connect;
        }

        public int Add(Genre genre)
        {
            using(var db = new Context(_connect))
            {
                db.Genres.Add(genre);
                db.SaveChanges();

                return db.Genres.AsNoTracking()
                        .Where(g => g.Genre_name == genre.Genre_name)
                        .Select(g => g.Id)
                        .FirstOrDefault();
            }
        }

        public int Delete(Genre genre)
        {
            using (var db = new Context(_connect))
            {
                var findGenre = db.Genres.AsNoTracking()
                    .FirstOrDefault(g => g.Genre_name == genre.Genre_name);

                if(findGenre != null)
                {
                    db.Genres.Remove(findGenre);
                    return db.SaveChanges();
                }

                return 0;
            }
        }

        public List<Genre> FindAll()
        {
            using (var db = new Context(_connect))
            {
                return db.Genres.ToList();
            }
        }

        public Genre? FindById(int id)
        {
            using (var db = new Context(_connect))
            {
                return db.Genres.FirstOrDefault(g => g.Id == id);
            }
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
