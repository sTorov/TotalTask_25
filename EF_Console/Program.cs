using EF_Console.Entity;
using EF_Console.Repository;

namespace EF_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new AppContext())
            {
                var user1 = new User { Name = "Dima", Email = "dima@email.com" };
                var user2 = new User { Name = "Kirill", Email = "kirill@email.com" };
                var user3 = new User { Name = "Anton", Email = "a@email.com" };
                var user4 = new User { Name = "User", Email = "user@email.com" };

                var book1 = new Book { Title = "Title_1", Year_of_issue = new DateTime(2000, 12, 1) };
                var book2 = new Book { Title = "Название_2", Year_of_issue = new DateTime(1992, 4, 23) };

                var genre1 = new Genre { Genre_name = "Comedy" };
                var genre2 = new Genre { Genre_name = "Detective" };

                var author1 = new Author { FirstName = "Name1", SecondName = "Surname1", LastName = "LastName1" };
                var author2 = new Author { FirstName = "Имя", SecondName = "Фамилия" };

                db.SaveChanges();
            }
        }
    }
}