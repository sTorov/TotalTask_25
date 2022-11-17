using EF_Console.Entity;
using EF_Console.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EF_Console.Repository
{
    public class BookRepository : IBookRepository
    {
        public int Add(Book book)
        {
            using (var db = new Context())
            {
                db.Books.Add(book);
                return db.SaveChanges();
            }
        }

        public int CountByAuthor(Author author)
        {
            using (var db = new Context())
            {
                return db.Books
                        .Where(b => b.Authors.Any(a => a == author)).Count();
            }
        }

        public int Delete(Book book)
        {
            using (var db = new Context())
            {
                var deletedBook = db.Books.AsNoTracking()
                    .FirstOrDefault(b => b.Title == book.Title && b.Year_of_issue == b.Year_of_issue);

                if (deletedBook != null)
                {
                    db.Books.Remove(deletedBook);
                    return db.SaveChanges();
                }

                return 0;
            }
        }

        public List<Book> FindAll()
        {
            using (var db = new Context())
            {
                return db.Books.ToList();
            }
        }

        public Book FindById(int id)
        {
            using (var db = new Context())
            {
                return db.Books.FirstOrDefault(book => book.Id == id);
            }
        }

        public List<Book> GetListByGenreAndYear(Genre genre, DateTime fromYear, DateTime toYear)
        {
            using (var context = new Context())
            {
                return context.Books
                            .Include(b => b.Genres).Include(b => b.Authors)
                                .Where(b => b.Genres.Any(g => g == genre) &&
                                    b.Year_of_issue >= fromYear &&
                                    b.Year_of_issue <= toYear)
                                .ToList();
            }
        }

        public int GetIdByTitleAndYear(Book book)
        {
            using (var db = new Context())
            {
                return db.Books.AsNoTracking().
                    Where(b => b.Title == book.Title && b.Year_of_issue == b.Year_of_issue)
                    .Select(b => b.Id).FirstOrDefault();
            }
        }

        public int UpdateYearOfIssueById(int id, DateTime date)
        {
            using (var db = new Context())
            {
                var updatedBook = db.Books.FirstOrDefault(book => book.Id == id);
                if (updatedBook != null)
                {
                    updatedBook.Year_of_issue = date;
                    return db.SaveChanges();
                }

                return 0;
            }
        }

        public int CountByGenre(Genre genre)
        {
            using (var context = new Context())
            {
                return context.Books
                    .Where(b => b.Genres.Any(g => g == genre))
                    .Count();
            }
        }

        public bool CheckByAuthorAndTitle(Author author, string title)
        {
            using (var db = new Context())
            {
                return db.Books.Any(b => b.Authors.Any(a => a == author) && b.Title == title);
            }
        }

        public int CountByUserId(User user)
        {
            using (var db = new Context())
            {
                return db.Books.Where(b => b.UserId == user.Id).Count();
            }
        }

        public Book GetByMaxYear()
        {
            using (var db = new Context())
            {
                return db.Books.Include(b => b.Authors)
                    .First(b => b.Year_of_issue == db.Books.Max(b => b.Year_of_issue));
            }
        }

        public List<Book> FindAllOrderByTitle()
        {
            using (var db = new Context())
            {
                return db.Books.Include(b => b.Authors)
                    .OrderBy(b => b.Title).ToList();
            }
        }

        public List<Book> FindAllOrderByDiscendingByYear()
        {
            using (var db = new Context())
            {
                return db.Books.Include(b => b.Authors)
                    .OrderByDescending(b => b.Year_of_issue).ToList();
            }
        }

        public Book FindByTitle(string title)
        {
            using(var db = new Context())
            {
                return db.Books.FirstOrDefault(b => b.Title == title);
            }
        }
    }

    interface IBookRepository
    {
        int Add(Book book);
        int Delete(Book book);
        List<Book> FindAll();
        List<Book> FindAllOrderByTitle();
        List<Book> FindAllOrderByDiscendingByYear();
        Book FindById(int id);
        Book FindByTitle(string title);
        int CountByAuthor(Author author);
        int CountByGenre(Genre genre);
        int CountByUserId(User user);
        bool CheckByAuthorAndTitle(Author author, string title);
        int GetIdByTitleAndYear(Book book);
        Book GetByMaxYear();
        List<Book> GetListByGenreAndYear(Genre genre, DateTime fromYear, DateTime toYear);
        int UpdateYearOfIssueById(int id, DateTime date);
    }
}
