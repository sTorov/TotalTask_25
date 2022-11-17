using EF_Console.Services;
using EF_Console.Configuration;

namespace EF_Console
{
    /// <summary>
    /// Основной класс
    /// </summary>
    class Program
    {
        /// <summary>
        /// Основной метод
        /// </summary>
        static void Main(string[] args)
        {
            if(!Start())
                return;            

            var service = new BookService();

            service.GetBooksByGenreAndDate("Detective", 1950, 2001, true);

            Console.WriteLine();
            service.GetBooksCountByAuthor("Игорь", "Петренко", printResult: true);
            service.GetBooksCountByAuthor(1, true);

            Console.WriteLine();
            service.GetBooksCountByGenre("Comedy", true);

            Console.WriteLine();
            service.CheckBookByTitleAndAuthor("Красивое название", "Иван", "Иванов", "Иванович", true);

            Console.WriteLine();
            service.CheckUserIsBook(1, 2, true);

            Console.WriteLine();
            service.CountBooksOnUser(4, true);

            Console.WriteLine();
            service.NewestBook(true);

            Console.WriteLine();
            service.AllBooksOrderByTitle(true);

            Console.WriteLine();
            service.AllBookOrderByDiscendingDate(true);


        }

        /// <summary>
        /// Вызов создания БД и инициализации в ней данных
        /// </summary>
        static bool Start()
        {
            try
            {
                InitialData.DB_InitializationData();
                return true;
            }
            catch (Exception e)
            {
                Helper.ErrorPrint(e.Message);
                Console.ReadKey();
                return false;
            }
        }
    }
}