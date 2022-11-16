namespace EF_Console.Entity
{
    public class Author
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string? LastName { get; set; }

        public List<Book> Books { get; set; }

        public override bool Equals(object? obj)
        {
            if(obj is Author author)
            {
                if(author.Id != Id) return false;
                if (author.FirstName != FirstName) return false;
                if (author.SecondName != SecondName) return false;
                if (author.LastName != LastName) return false;

                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return $"{SecondName} {FirstName} {LastName}";
        }
    }
}
