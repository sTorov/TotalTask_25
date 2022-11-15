namespace EF_Console.Entity
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Year_of_issue { get; set; }

        public override bool Equals(object? obj)
        {
            if(obj is Book book)
            {
                if (book.Id != Id) return false;
                if (book.Title != Title) return false;
                if (book.Year_of_issue != Year_of_issue) return false;
                return true;
            }
            return false;
        }
    }
}
