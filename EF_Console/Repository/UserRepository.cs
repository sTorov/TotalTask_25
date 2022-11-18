using EF_Console.Entity;
using EF_Console.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EF_Console.Repository
{
    /// <summary>
    /// Репозиторий пользователя
    /// </summary>
    public class UserRepository : IUserRepository, IRepository<User>
    {
        private string _connect;

        public UserRepository(string connect)
        {
            _connect = connect;
        }

        public int Add(User user)
        {
            using(var db = new Context(_connect))
            {
                db.Users.Add(user);
                db.SaveChanges();

                return db.Users.AsNoTracking()
                        .Where(u => u.Email == user.Email)
                        .Select(u => u.Id)
                        .FirstOrDefault();
            }
        }

        public int Delete(User user)
        {
            using (var db = new Context(_connect))
            {
                var deletedUser = db.Users.AsNoTracking()
                    .FirstOrDefault(u => u.Email == user.Email);

                if(deletedUser != null)
                {
                    db.Users.Remove(user);
                    return db.SaveChanges();
                }

                return 0;
            }
        }

        public List<User> FindAll()
        {
            using (var db = new Context(_connect))
            {
                return db.Users.ToList();
            }
        }

        public User? FindById(int id)
        {
            using (var db = new Context(_connect))
            {
                return db.Users.FirstOrDefault(user => user.Id == id);
            }
        }

        public int UpdateUserNameById(int id, string newName)
        {
            using (var db = new Context(_connect))
            {
                var updatedUser = db.Users.FirstOrDefault(user => user.Id == id);
                if(updatedUser != null)
                {
                    updatedUser.Name = newName;
                    return db.SaveChanges();
                }

                return 0;
            }
        }
    }

    /// <summary>
    /// Интерфейс репозитория пользователя
    /// </summary>
    interface IUserRepository
    {
        /// <summary>
        /// Обновление имени пользователя по Id
        /// </summary>
        int UpdateUserNameById(int id, string newName);
    }
}
