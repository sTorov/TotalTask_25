using EF_Console.Configuration;
using EF_Console.Entity;
using EF_Console.Repository;

namespace EF_Console.Services
{
    /// <summary>
    /// Сервис книг
    /// </summary>
    public class BookService
    {
        private string _connect;
        private AuthorRepository authorRepository;
        private GenreRepository genreRepository;
        private UserRepository userRepository;
        private BookRepository bookRepository;

        /// <summary>
        /// Конструктор сервиса книг
        /// </summary>
        public BookService(string connect = ConnectionString.MAIN)
        {
            _connect = connect;
            authorRepository = new AuthorRepository(_connect);
            genreRepository = new GenreRepository(_connect);
            userRepository = new UserRepository(_connect);
            bookRepository = new BookRepository(_connect);
        }


        /// <summary>
        /// Получение списка книг по жанру, выпущенных в определённый промежуток времени
        /// </summary>
        public List<Book> GetBooksByGenreAndDate(string genre_name, int fromYear, int toYear, bool printResult = false)
        {
            try
            {
                if (fromYear >= toYear)
                    throw new Exception("GetBooksByGenreAndDate: Получены некорректыне данные!");

                var genre = genreRepository.FindByName(genre_name);
                if (genre == null)
                    throw new Exception($"GetBooksByGenreAndDate: Жанр [{genre_name}] не найден!");

                var bookList = bookRepository.GetListByGenreAndYear(genre, new DateTime(fromYear, 1, 1), new DateTime(toYear, 1, 1));

                if(printResult)
                    Helper.PrintList(bookList, $"Список книг жанра {genre_name} c {fromYear} по {toYear} годы:");

                return bookList;
            }
            catch (Exception e)
            {
                Helper.ErrorPrint(e.Message);
                return null;
            }
        }


        #region BookCount

        /// <summary>
        /// Получение количества книг, написанных определеённым автором, по его Id
        /// </summary>
        public int GetBooksCountByAuthor(int authorId, bool printResult = false)
        {
            try
            {
                var author = authorRepository.FindById(authorId);
                if (author == null)
                    throw new Exception($"GetBooksCountByAuthor: Автора с Id [{authorId}] не существует!");

                var bookCount = bookRepository.CountByAuthor(author);

                if (printResult)
                    Console.WriteLine($"{author.SecondName} {author.FirstName} {author.LastName ?? "_"}" +
                        $" написал книг: {bookCount}");

                return bookCount;
            }
            catch (Exception e)
            {
                Helper.ErrorPrint(e.Message);
                return 0;
            }
        }

        /// <summary>
        /// Получение количества книг, написанных определеённым автором, по его полному имени
        /// </summary>
        public int GetBooksCountByAuthor(string firstName, string secondName, string lastName = null, bool printResult = false)
        {
            try
            {
                var author = authorRepository.FindByFullName(firstName, secondName, lastName);
                if (author == null)
                    throw new Exception($"GetBooksCountByAuthor: Автора [{secondName} {firstName} {lastName ?? "_"}] не существует!");

                var bookCount = bookRepository.CountByAuthor(author);

                if (printResult)
                    Console.WriteLine($"{author.SecondName} {author.FirstName} {author.LastName ?? "_"}" +
                        $" написал книг: {bookCount}");

                return bookCount;
            }
            catch (Exception e)
            {
                Helper.ErrorPrint(e.Message);
                return 0;
            }
        }

        #endregion


        /// <summary>
        /// Получение количества книг определённого жанра
        /// </summary>
        public int GetBooksCountByGenre(string genre_name, bool printResult = false)
        {
            try
            {
                var genre = genreRepository.FindByName(genre_name);

                if (genre == null)
                    throw new Exception($"GetBooksCountByGenre: Жанр [{genre_name}] не найден!");

                int bookCount = bookRepository.CountByGenre(genre);

                if(printResult)
                    Console.WriteLine($"Количество книг жанра {genre_name}: {bookCount}");

                return bookCount;
            }
            catch (Exception e)
            {
                Helper.ErrorPrint(e.Message);
                return 0;
            }
        }

        /// <summary>
        /// Проверка автора книги
        /// </summary>
        public bool CheckBookByTitleAndAuthor(string title, string firstName, string secondName, string lastName = null, bool printResult = false)
        {
            try
            {
                if (bookRepository.FindByTitle(title) == null)
                    throw new Exception($"CheckBookByTitleAndAuthor: Книга [{title}] не найдена!");

                var author = authorRepository.FindByFullName(firstName, secondName, lastName);

                if (author == null)
                    throw new Exception($"CheckBookByTitleAndAuthor: " +
                        $"Автор [{secondName} {firstName} {lastName ?? "_"}] не найден!");

                var check = bookRepository.CheckByAuthorAndTitle(author, title);

                if(printResult)
                    Console.WriteLine($"Проверка: Автор -> Книга\n{secondName} {firstName} {lastName} ->  {title}  >>  Результат: {check}");

                return check;
            }
            catch (Exception e)
            {
                Helper.ErrorPrint(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Проверка наличия у пользователя книги
        /// </summary>
        public bool CheckUserIsBook(int userId, int bookId, bool printResult = false)
        {
            try
            {
                var user = userRepository.FindById(userId);
                if (user == null)
                    throw new Exception($"CheckUserIsBook: Пользователь с Id [{userId}] не найден!");

                var book = bookRepository.FindById(bookId);
                if(book == null)
                    throw new Exception($"CheckUserIsBook: Книга с Id [{bookId}] не найдена!");

                var check = book.UserId == user.Id;

                if(printResult)
                    Console.WriteLine($"Проверка: Пользователь -> Книга\n{user.Name} ->  {book.Title}  >>  Результат: {check}");

                return check;
            }
            catch (Exception e)
            {
                Helper.ErrorPrint(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Получение количества книг у пользователя
        /// </summary>
        public int CountBooksOnUser(int userId, bool printResult = false)
        {
            try
            {
                var user = userRepository.FindById(userId);

                if (user == null)
                    throw new Exception($"CountBooksOnUser: Пользователь с Id [{userId}] не найден!");

                int count = bookRepository.CountByUserId(user);

                if(printResult)
                    Console.WriteLine($"Количество книг у пользователя {user.Name}: {count}");

                return count;
            }
            catch (Exception e)
            {
                Helper.ErrorPrint(e.Message);
                return 0;
            }
        }

        /// <summary>
        /// Получение книги, выпущеной позденее всех
        /// </summary>
        public Book NewestBook(bool printResult = false)
        {
            try
            {
                var book = bookRepository.GetByMaxYear();
                if (book == null)
                    throw new Exception("NewestBook: Ошибка при получении книги!");

                if (printResult)
                    Console.WriteLine($"Самая новая книга:\n{book}");

                return book;
            }
            catch (Exception e)
            {
                Helper.ErrorPrint(e.Message);
                return null;
            }
        }

        /// <summary>
        /// Получение списка всех книг, отсортированных по названии в порядке возрастания
        /// </summary>
        public List<Book> AllBooksOrderByTitle(bool printResult = false)
        {
            try
            {
                var bookList = bookRepository.FindAllOrderByTitle();
                if(bookList == null)
                    throw new Exception($"AllBooksOrderByTitle: Ошибка при получении списка книг!");

                if (printResult)
                    Helper.PrintList(bookList, "Все книги, сортировка по назнанию - возрастание:");

                return bookList;
            }
            catch (Exception e)
            {
                Helper.ErrorPrint(e.Message);
                return null;
            }
        }

        /// <summary>
        /// Получение списка всех книг, отсортированных по дате издания в порядке убывания
        /// </summary>
        public List<Book> AllBookOrderByDiscendingDate(bool printResult = false)
        {
            try
            {
                var bookList = bookRepository.FindAllOrderByDiscendingByYear();
                if (bookList == null)
                    throw new Exception("AllBookOrderByDiscendingDate: ");

                if (printResult)
                    Helper.PrintList(bookList, "Все книги, сортировка по дате издания - убывание:");

                return bookList;
            }
            catch (Exception e)
            {
                Helper.ErrorPrint(e.Message);
                return null;
            }
        }
    }
}
