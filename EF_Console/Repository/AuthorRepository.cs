using EF_Console.Entity;
using EF_Console.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EF_Console.Repository
{
    /// <summary>
    /// Репозиторий автора
    /// </summary>
    public class AuthorRepository : IAuthorRepository, IRepository<Author>
    {
        private string _connect;

        public AuthorRepository(string connect)
        {
            _connect = connect;
        }

        public int Add(Author author)
        {
            using(var db = new Context(_connect))
            {
                db.Authors.Add(author);
                db.SaveChanges();

                return db.Authors.AsNoTracking().
                        Where(a => a.FirstName == author.FirstName &&
                                a.SecondName == author.SecondName &&
                                a.LastName == author.LastName)
                        .Select(a => a.Id)
                        .FirstOrDefault();
            }
        }

        public int Delete(Author author)
        {
            using(var db = new Context(_connect))
            {
                var findAuthor = db.Authors.AsNoTracking()
                    .FirstOrDefault(a => a.FirstName == author.FirstName &&
                                        a.SecondName == author.SecondName &&
                                        a.LastName == author.LastName);

                if(findAuthor != null)
                {
                    db.Authors.Remove(findAuthor);
                    return db.SaveChanges();
                }

                return 0;
            }
        }

        public List<Author> FindAll()
        {
            using(var db = new Context(_connect))
            {
                return db.Authors.ToList();
            }
        }

        public Author? FindByFullName(string firstName, string secondName, string lastName)
        {
            using(var db = new Context(_connect))
            {
                return db.Authors.AsNoTracking()
                    .FirstOrDefault(a => a.FirstName == firstName &&
                                                    a.SecondName == secondName &&
                                                    a.LastName == lastName);
            }
        }

        public Author? FindById(int id)
        {
            using (var db = new Context(_connect))
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
    }
}
