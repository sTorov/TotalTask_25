using EF_Console.Entity;
using EF_Console.Repository;

namespace EF_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //using(var db = new AppContext())
            //{
            //    var user1 = new User { Name = "Dima", Email = "dima@email.com" };
            //    var user2 = new User { Name = "Kirill", Email = "kirill@email.com" };

            //    db.Users.Add(user1);
            //    db.Users.Add(user2);

            var book1 = new Book { Title = "Title_1", Year_of_issue = new DateTime(2000, 12, 1) };
            //    var book2 = new Book { Title = "Название_2", Year_of_issue = new DateTime(1992, 4, 23) };

            //    db.Books.Add(book1);
            //    db.Books.Add(book2);

            //    db.SaveChanges();
            //}

            BookRepository bookRepository = new BookRepository();
            //bookRepository.Add(new Book { Title = "Title_1", Year_of_issue = new DateTime(2000, 12, 1) });
            //bookRepository.Add(new Book { Title = "Название_2", Year_of_issue = new DateTime(1992, 4, 23) });
            bookRepository.Delete(book1);
            Console.ReadKey();
        }
    }
}