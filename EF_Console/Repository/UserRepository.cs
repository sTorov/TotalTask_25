using EF_Console.Entity;
using EF_Console.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EF_Console.Repository
{
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

        public int GetIdByEmailAndName(User user)
        {
            using(var db = new Context())
            {
                return db.Users.AsNoTracking()
                    .Where(u => u.Email == user.Email && u.Name == user.Name)
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

    interface IUserRepository
    {
        User FindById(int id);
        List<User> FindAll();
        int Add(User user);
        int Delete(User user);
        int UpdateUserNameById(int id, string newName);
        int GetIdByEmailAndName(User user);
    }
}
