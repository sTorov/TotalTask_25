using EF_Console.Entity;
using EF_Console.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EF_Console.Repository
{
    /// <summary>
    /// Репозиторий книги
    /// </summary>
    public class BookRepository : IBookRepository
    {
        private string _connect;

        public BookRepository(string connect)
        {
            _connect = connect;
        }

        public int Add(Book book)
        {
            using (var db = new Context(_connect))
            {
                db.Books.Add(book);
                return db.SaveChanges();
            }
        }

        public int CountByAuthor(Author author)
        {
            using (var db = new Context(_connect))
            {
                return db.Books
                        .Where(b => b.Authors.Any(a => a == author)).Count();
            }
        }

        public int Delete(Book book)
        {
            using (var db = new Context(_connect))
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
            using (var db = new Context(_connect))
            {
                return db.Books.ToList();
            }
        }

        public Book FindById(int id)
        {
            using (var db = new Context(_connect))
            {
                return db.Books.FirstOrDefault(book => book.Id == id);
            }
        }

        public List<Book> GetListByGenreAndYear(Genre genre, DateTime fromYear, DateTime toYear)
        {
            using (var context = new Context(_connect))
            {
                return context.Books
                            .Include(b => b.Genres).Include(b => b.Authors)
                                .Where(b => b.Genres.Any(g => g == genre) &&
                                    b.Year_of_issue >= fromYear &&
                                    b.Year_of_issue <= toYear)
                                .ToList();
            }
        }

        public int GetIdByTitleAndYear(string title, DateTime date)
        {
            using (var db = new Context(_connect))
            {
                return db.Books.AsNoTracking().
                    Where(b => b.Title == title && b.Year_of_issue == date)
                    .Select(b => b.Id).FirstOrDefault();
            }
        }

        public int UpdateYearOfIssueById(int id, DateTime date)
        {
            using (var db = new Context(_connect))
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
            using (var context = new Context(_connect))
            {
                return context.Books
                    .Where(b => b.Genres.Any(g => g == genre))
                    .Count();
            }
        }

        public bool CheckByAuthorAndTitle(Author author, string title)
        {
            using (var db = new Context(_connect))
            {
                return db.Books.Any(b => b.Authors.Any(a => a == author) && b.Title == title);
            }
        }

        public int CountByUserId(User user)
        {
            using (var db = new Context(_connect))
            {
                return db.Books.Where(b => b.UserId == user.Id).Count();
            }
        }

        public Book GetByMaxYear()
        {
            using (var db = new Context(_connect))
            {
                return db.Books.Include(b => b.Authors)
                    .First(b => b.Year_of_issue == db.Books.Max(b => b.Year_of_issue));
            }
        }

        public List<Book> FindAllOrderByTitle()
        {
            using (var db = new Context(_connect))
            {
                return db.Books.Include(b => b.Authors)
                    .OrderBy(b => b.Title).ToList();
            }
        }

        public List<Book> FindAllOrderByDiscendingByYear()
        {
            using (var db = new Context(_connect))
            {
                return db.Books.Include(b => b.Authors)
                    .OrderByDescending(b => b.Year_of_issue).ToList();
            }
        }

        public Book FindByTitle(string title)
        {
            using(var db = new Context(_connect))
            {
                return db.Books.FirstOrDefault(b => b.Title == title);
            }
        }
    }

    /// <summary>
    /// Интерфейс репозитория книги
    /// </summary>
    interface IBookRepository
    {
        /// <summary>
        /// Добавление новой книги в БД
        /// </summary>
        int Add(Book book);
        /// <summary>
        /// Удаление книги из БД
        /// </summary>
        int Delete(Book book);
        /// <summary>
        /// Получение списка всех книг
        /// </summary>
        List<Book> FindAll();
        /// <summary>
        /// Получение списка всех книг, сортировка по названию - возрастание
        /// </summary>
        List<Book> FindAllOrderByTitle();
        /// <summary>
        /// Получение списка всех книг, сортировка по году издания - убывание
        /// </summary>
        List<Book> FindAllOrderByDiscendingByYear();
        /// <summary>
        /// Получение книги по Id
        /// </summary>
        Book FindById(int id);
        /// <summary>
        /// Получение книги по названию
        /// </summary>
        Book FindByTitle(string title);
        /// <summary>
        /// Получение количества книг, написанных указанным автором
        /// </summary>
        int CountByAuthor(Author author);
        /// <summary>
        /// Получение количества книг указанного жанра
        /// </summary>
        int CountByGenre(Genre genre);
        /// <summary>
        /// Получение количества книг, находящихся на руках у пользователя
        /// </summary>
        int CountByUserId(User user);
        /// <summary>
        /// Проверка автора книги
        /// </summary>
        bool CheckByAuthorAndTitle(Author author, string title);
        /// <summary>
        /// Получение Id по названию книги и году выпуска
        /// </summary>
        int GetIdByTitleAndYear(string title, DateTime date);
        /// <summary>
        /// Получение книги, выпущеной позднее всех
        /// </summary>
        Book GetByMaxYear();
        /// <summary>
        /// Получение списка книг по указанному жанру, выпущеных в определённый промежуток времени 
        /// </summary>
        List<Book> GetListByGenreAndYear(Genre genre, DateTime fromYear, DateTime toYear);
        /// <summary>
        /// Обновление даты выпуска книги по Id
        /// </summary>
        int UpdateYearOfIssueById(int id, DateTime date);
    }
}
