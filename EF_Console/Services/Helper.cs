using EF_Console.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Console.Services
{
    public class Helper
    {
        /// <summary>
        /// Вывод списка книг в консоль
        /// </summary>
        public static void PrintBookList(IEnumerable<Book> list)
        {
            if (list != null)
                foreach (var book in list)
                    Console.WriteLine(book);
            else
                Console.WriteLine("Пусто");
        }
    }
}
