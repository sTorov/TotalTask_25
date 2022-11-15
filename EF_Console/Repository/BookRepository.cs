using EF_Console.Entity;
using Microsoft.EntityFrameworkCore;

namespace EF_Console.Repository
{
    public class BookRepository : IBookRepository
    {
        public int Add(Book book)
        {
            using(var db = new AppContext())
            {
                db.Books.Add(book);
                return db.SaveChanges();
            }
        }

        public int Delete(Book book)
        {
            using(var db = new AppContext())
            {
                var deletedBook = db.Books.AsNoTracking()
                    .FirstOrDefault(b => b.Title == book.Title && b.Year_of_issue == b.Year_of_issue);

                if(deletedBook != null)
                {
                    db.Books.Remove(deletedBook);
                    return db.SaveChanges();
                }

                return 0;
            }
        }

        public List<Book> FindAll()
        {
            using (var db = new AppContext())
            {
                return db.Books.ToList();
            }
        }

        public Book FindById(int id)
        {
            using (var db = new AppContext())
            {
                return db.Books.FirstOrDefault(book => book.Id == id);
            }
        }

        public int UpdateYearOfIssueById(int id, DateTime date)
        {
            using (var db = new AppContext())
            {
                var updatedBook = db.Books.FirstOrDefault(book => book.Id == id);
                if(updatedBook != null)
                {
                    updatedBook.Year_of_issue = date;
                    return db.SaveChanges();
                }

                return 0;
            }
        }
    }

    interface IBookRepository
    {
        Book FindById(int id);
        List<Book> FindAll();
        int Add(Book book);
        int Delete(Book book);
        int UpdateYearOfIssueById(int id, DateTime date);
    }
}
