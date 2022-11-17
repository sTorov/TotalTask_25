using EF_Console.Services;
using EF_Console.Configuration;

namespace EF_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            if(!Start())
                return;
            
            var service = new BookService();

            service.GetBooksByGenreAndDate("Detective", 1950, 2001, true);

            Console.WriteLine();
            service.GetBooksCountByAuthor("Имя", "Фамилия", printResult: true);
            service.GetBooksCountByAuthor(1, true);

            Console.WriteLine();
            service.GetBooksCountByGenre("Comedy", true);

            Console.WriteLine();
            service.CheckBookByTitleAndAuthor("Title_1", "Name1", "Surname1", "LastName1", true);

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
        /// Вызов создания БД и инициализация в ней данных
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

        //static void Method()
        //{
        //    using(var db = new Context())
        //    {
        //        using (var trans = db.Database.BeginTransaction())
        //        {
        //            var user = new User { Id = 99, Email = "test", Name = "Test" };
        //            db.Users.Add(user);

        //            db.Database.ExecuteSqlInterpolated($"set identity_insert dbo.Users on;");
        //            db.SaveChanges();
        //            db.Database.ExecuteSqlInterpolated($"set identity_insert dbo.Users off;");

        //            trans.Commit();
        //        }
        //    }
        //}
    }
}