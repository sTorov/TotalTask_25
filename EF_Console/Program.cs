using EF_Console.Entity;
using EF_Console.Services;
using System.Collections.Generic;

namespace EF_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new BookService();

            try
            {
                DB_InitializationData();

                PrintBookList(service.GetBooksByGenreAndDate("Detective", 1950, 2001));

                Console.WriteLine(service.GetBooksCountByAuthor("Имя", "Фамилия"));
                Console.WriteLine(service.GetBooksCountByAuthor(1));

                Console.WriteLine(service.GetBooksCountByGenre("Comedy"));

                Console.WriteLine(service.CheckBookByTitleAndAuthor("Title_1", "Name1", "Surname1","LastName1"));

                Console.WriteLine(service.CheckUserIsBook(1, 2));

                Console.WriteLine(service.CountBooksOnUser(4));

                Console.WriteLine(service.NewestBook());

                Console.WriteLine();
                PrintBookList(service.AllBooksOrderByTitle());
                Console.WriteLine();
                PrintBookList(service.AllBookOrderByDiscendingDate());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Вывод списка книг в консоль
        /// </summary>
        static void PrintBookList(IEnumerable<Book> list)
        {
            if (list != null)
                foreach (var book in list)
                    Console.WriteLine(book);
            else
                Console.WriteLine("Пусто");
        }

        /// <summary>
        /// Создание БД и инициализация в ней данных
        /// </summary>
        static void DB_InitializationData()
        {
            using (var context = new AppContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var user1 = new User { Name = "Dima", Email = "dima@email.com" };
                var user2 = new User { Name = "Kirill", Email = "kirill@email.com" };
                var user3 = new User { Name = "Anton", Email = "a@email.com" };
                var user4 = new User { Name = "User", Email = "user@email.com" };

                context.Users.AddRange(user1, user2, user3, user4);

                var book1 = new Book { Title = "Title_1", Year_of_issue = new DateTime(2000, 12, 1) };
                var book2 = new Book { Title = "Название_2", Year_of_issue = new DateTime(1992, 4, 23) };
                var book3 = new Book { Title = "Очень старая книга", Year_of_issue = new DateTime(1966, 2, 23) };
                var book4 = new Book { Title = "Не интересная книга", Year_of_issue = new DateTime(2022, 10, 1) };

                context.Books.AddRange(book1, book2, book3, book4);

                var genre1 = new Genre { Genre_name = "Comedy" };
                var genre2 = new Genre { Genre_name = "Detective" };

                context.Genres.AddRange(genre1, genre2);

                var author1 = new Author { FirstName = "Name1", SecondName = "Surname1", LastName = "LastName1" };
                var author2 = new Author { FirstName = "Имя", SecondName = "Фамилия" };

                context.Authors.AddRange(author1, author2);

                book1.User = user1;
                book2.User = user4;
                book3.User = user4;

                book1.Authors = new List<Author> { author1, author2 };
                book2.Authors = new List<Author> { author2 };
                book3.Authors = new List<Author> { author2 };
                book4.Authors = new List<Author> { author1 };

                book1.Genres = new List<Genre> { genre1, genre2 };
                book2.Genres = new List<Genre> { genre1 };
                book3.Genres = new List<Genre> { genre2 };
                book4.Genres = new List<Genre> { genre1, genre2 };

                context.SaveChanges();
            }
        }
    }
}