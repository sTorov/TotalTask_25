using EF_Console.Entity;
using Microsoft.EntityFrameworkCore;

namespace EF_Console.Repository
{
    public class GenreRepository : IGenreRepository
    {
        public Genre? FindByName(string name)
        {
            using(var db = new AppContext())
            {
                return db.Genres.AsNoTracking().FirstOrDefault(g => g.Genre_name == name);
            }
        }
    }

    interface IGenreRepository
    {
        Genre FindByName(string name);
    }
}
