using EF_Console.Entity;
using EF_Console.Repository;

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

        public List<Book> GetBooksByGenreAndDate(string genre_name, int fromYear, int toYear)
        {
            var genre = genreRepository.FindByName(genre_name);

            if (fromYear >= toYear || genre == null)
                return null;

            return bookRepository.GetListByGenreAndYear(genre, new DateTime(fromYear, 1, 1), new DateTime(toYear, 1, 1));
        }

        #region BookCount

        public int GetBooksCountByAuthor(int authorId)
        {
            var author = authorRepository.FindById(authorId);

            if(author == null)
                return 0;

            return bookRepository.CountByAuthor(author);
        }

        public int GetBooksCountByAuthor(string firstName, string secondName, string lastName = null)
        {
            var author = authorRepository.FindByFullName(firstName, secondName, lastName);

            if (author == null)
                return 0;

            return bookRepository.CountByAuthor(author);
        }

        #endregion

        public int GetBooksCountByGenre(string genre_name)
        {
            var genre = genreRepository.FindByName(genre_name);

            if (genre == null)
                return 0;

            return bookRepository.CountByGenre(genre);
        }

        public bool CheckBookByTitleAndAuthor(string title, string firstName, string secondName, string lastName = null)
        {
            var author = authorRepository.FindByFullName(firstName, secondName, lastName);

            if (author == null)
                return false;

            return bookRepository.CheckByAuthorAndTitle(author, title);
        }

        public bool CheckUserIsBook(int userId, int bookId)
        {
            var user = userRepository.FindById(userId);

            if (user == null)
                return false;

            return bookRepository.FindById(bookId).UserId == user.Id;
        }

        public int CountBooksOnUser(int userId)
        {
            var user = userRepository.FindById(userId);

            if (user == null)
                return 0;

            return bookRepository.CountByUserId(user);
        }

        public Book NewestBook()
        {
            return bookRepository.GetByMaxYear();
        }

        public List<Book> AllBooksOrderByTitle()
        {
            return bookRepository.FindAllOrderByTitle();
        }

        public List<Book> AllBookOrderByDiscendingDate()
        {
            return bookRepository.FindAllOrderByDiscendingByYear();
        }
    }
}
