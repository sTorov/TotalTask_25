using EF_Console.Entity;
using Microsoft.EntityFrameworkCore;

namespace EF_Console.Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        public Author? FindByFullName(string firstName, string secondName, string lastName)
        {
            using(var db = new AppContext())
            {
                return db.Authors.AsNoTracking()
                    .FirstOrDefault(a => a.FirstName == firstName &&
                                                    a.SecondName == secondName &&
                                                    a.LastName == lastName);
            }
        }

        public Author? FindById(int id)
        {
            using (var db = new AppContext())
            {
                return db.Authors.AsNoTracking()
                    .FirstOrDefault(a => a.Id == id);
            }
        }
    }

    interface IAuthorRepository
    {
        Author FindByFullName(string firstName, string secondName, string lastName);
        Author FindById(int id);
    }
}
