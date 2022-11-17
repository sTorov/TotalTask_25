using EF_Console.Entity;
using EF_Console.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EF_Console.Repository
{
    /// <summary>
    /// Репозиторий автора
    /// </summary>
    public class AuthorRepository : IAuthorRepository
    {
        public Author? FindByFullName(string firstName, string secondName, string lastName)
        {
            using(var db = new Context())
            {
                return db.Authors.AsNoTracking()
                    .FirstOrDefault(a => a.FirstName == firstName &&
                                                    a.SecondName == secondName &&
                                                    a.LastName == lastName);
            }
        }

        public Author? FindById(int id)
        {
            using (var db = new Context())
            {
                return db.Authors.AsNoTracking()
                    .FirstOrDefault(a => a.Id == id);
            }
        }
    }

    /// <summary>
    /// Интерфейс репозитория автора
    /// </summary>
    interface IAuthorRepository
    {
        /// <summary>
        /// Получение автора по полному имени 
        /// </summary>
        Author? FindByFullName(string firstName, string secondName, string lastName);
        /// <summary>
        /// Получение автора по Id
        /// </summary>
        Author? FindById(int id);
    }
}
