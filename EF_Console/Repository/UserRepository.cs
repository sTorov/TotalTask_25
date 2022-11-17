using EF_Console.Entity;
using EF_Console.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EF_Console.Repository
{
    /// <summary>
    /// Репозиторий пользователя
    /// </summary>
    public class UserRepository : IUserRepository
    {
        public int Add(User user)
        {
            using(var db = new Context())
            {
                db.Users.Add(user);
                return db.SaveChanges();
            }
        }

        public int Delete(User user)
        {
            using (var db = new Context())
            {
                var deletedUser = db.Users.AsNoTracking().FirstOrDefault(u => u.Name == user.Name && u.Email == user.Email);
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
            using (var db = new Context())
            {
                return db.Users.ToList();
            }
        }

        public User FindById(int id)
        {
            using (var db = new Context())
            {
                return db.Users.FirstOrDefault(user => user.Id == id);
            }
        }

        public int GetIdByEmailAndName(string name, string email)
        {
            using(var db = new Context())
            {
                return db.Users.AsNoTracking()
                    .Where(u => u.Email == email && u.Name == name)
                    .Select(u => u.Id).FirstOrDefault();
            }
        }

        public int UpdateUserNameById(int id, string newName)
        {
            using (var db = new Context())
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
        /// Получение пользователя по Id
        /// </summary>
        User FindById(int id);
        /// <summary>
        /// Получение списка всех пользователей
        /// </summary>
        List<User> FindAll();
        /// <summary>
        /// Добавление пользователя в БД
        /// </summary>
        int Add(User user);
        /// <summary>
        /// Удаление пользователя из БД
        /// </summary>
        int Delete(User user);
        /// <summary>
        /// Обновление имени пользователя по Id
        /// </summary>
        int UpdateUserNameById(int id, string newName);
        /// <summary>
        /// Получение Id по почтовому адресу и имени пользователя
        /// </summary>
        int GetIdByEmailAndName(string name, string email);
    }
}
