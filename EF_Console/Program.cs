using EF_Console.Entity;
using EF_Console.Services;
using EF_Console.Configuration;

namespace EF_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new BookService();

            try
            {
                InitialData.DB_InitializationData();

                Helper.PrintBookList(service.GetBooksByGenreAndDate("Detective", 1950, 2001));

                Console.WriteLine(service.GetBooksCountByAuthor("Имя", "Фамилия"));
                Console.WriteLine(service.GetBooksCountByAuthor(1));

                Console.WriteLine(service.GetBooksCountByGenre("Comedy"));

                Console.WriteLine(service.CheckBookByTitleAndAuthor("Title_1", "Name1", "Surname1", "LastName1"));

                Console.WriteLine(service.CheckUserIsBook(1, 2));

                Console.WriteLine(service.CountBooksOnUser(4));

                Console.WriteLine(service.NewestBook());

                Console.WriteLine();
                Helper.PrintBookList(service.AllBooksOrderByTitle());

                Console.WriteLine();
                Helper.PrintBookList(service.AllBookOrderByDiscendingDate());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }        
    }
}