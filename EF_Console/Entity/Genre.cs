using System.ComponentModel.DataAnnotations.Schema;

namespace EF_Console.Entity
{
    /// <summary>
    /// Сущность жанра
    /// </summary>
    public class Genre
    {
        public int Id { get; set; }
        public string Genre_name { get; set; }

        public List<Book> Books { get; set; }
    }
}
