using EF_Console.Entity;
using EF_Console.Repository;
using Microsoft.EntityFrameworkCore;

namespace EF_Console.Services
{
    public class BookService
    {
        private AuthorRepository authorRepository;
        private GenreRepository genreRepository;
        private UserRepository userRepository;
        private BookRepository bookRepository;

        public BookService()
        {
            authorRepository = new AuthorRepository();
            genreRepository = new GenreRepository();
            userRepository = new UserRepository();
            bookRepository = new BookRepository();
        }

        public List<Book> GetBooksByGenreAndDate(string genre, int fromYear, int toYear)
        {
            if (fromYear >= toYear || genreRepository.FindByName(genre) == null)
                return null;

            using (var context = new AppContext())
            {
                return context.Books
                            .Include(b => b.Genres).Include(b => b.Authors)
                                .Where(b => b.Genres.Any(g => g.Genre_name == genre) &&
                                    b.Year_of_issue >= new DateTime(fromYear, 1, 1) &&
                                    b.Year_of_issue <= new DateTime(toYear, 1, 1))
                                .ToList();
            }            
        }

        #region BookCount

        public int GetBooksCountByAuthor(int authorId)
        {
            var author = authorRepository.FindById(authorId);

            if(author == null)
                return 0;

            return BooksCountByAuthor(author);
        }

        public int GetBooksCountByAuthor(string firstName, string secondName, string lastName = null)
        {
            var author = authorRepository.FindByFullName(firstName, secondName, lastName);

            if (author == null)
                return 0;

            return BooksCountByAuthor(author);
        }

        private int BooksCountByAuthor(Author author)
        {
            using (var context = new AppContext())
            {
                return context.Books
                        .Where(b => b.Authors.Any(a => a == author)).Count();
            }
        }

        #endregion

        public int GetBooksCountByGenre(string genre)
        {
            if (genreRepository.FindByName(genre) == null)
                return 0;

            using(var context = new AppContext())
            {
                return context.Books
                    .Where(b => b.Genres.Any(g => g.Genre_name == genre))
                    .Count();
            }
        }

        public bool CheckBookByTitleAndAuthor(string title, string firstName, string secondName, string lastName = null)
        {
            var author = authorRepository.FindByFullName(firstName, secondName, lastName);

            if (author == null)
                return false;

            using (var db = new AppContext())
            {
                return db.Books.Any(b => b.Authors.Any(a => a == author) && b.Title == title);
            }
        }

        public bool CheckUserIsBook(int userId, int bookId)
        {
            if (userRepository.FindById(userId) == null)
                return false;

            return bookRepository.FindById(bookId).UserId == userId;
        }

        public int CountBooksOnUser(int userId)
        {
            if (userRepository.FindById(userId) == null)
                return 0;

            using(var db = new AppContext())
            {
                return db.Books.Where(b => b.UserId == userId).Count();
            }
        }

        public Book NewestBook()
        {
            using (var db = new AppContext())
            {
                return db.Books.Include(b => b.Authors)
                    .First(b => b.Year_of_issue == db.Books.Max(b => b.Year_of_issue));
            }
        }

        public List<Book> AllBooksOrderByTitle()
        {
            using(var db = new AppContext())
            {
                return db.Books.Include(b => b.Authors)
                    .OrderBy(b => b.Title).ToList();
            }
        }

        public List<Book> AllBookOrderByDiscendingDate()
        {
            using(var db = new AppContext())
            {
                return db.Books.Include(b => b.Authors)
                    .OrderByDescending(b => b.Year_of_issue).ToList();
            }
        }
    }
}
