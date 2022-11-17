namespace EF_Console.Services
{
    /// <summary>
    /// Класс с вспомогательным функционалом
    /// </summary>
    public class Helper
    {
        /// <summary>
        /// Вывод списка в консоль
        /// </summary>
        public static void PrintList<T>(List<T> list, string title = null)
        {
            if(title != null)
                Console.WriteLine(title);

            if (list != null && list.Count() > 0)
                for(int i = 0; i < list.Count(); i++)
                    Console.WriteLine($"  {i + 1}. {list[i]}");
            else
                Console.WriteLine("Список пуст!");
        }

        /// <summary>
        /// Вывод сообщения об ошибке в консоль
        /// </summary>
        public static void ErrorPrint(string message)
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
