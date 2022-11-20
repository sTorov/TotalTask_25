using EF_Console.Configuration;
using EF_Console.Entity;
using EF_Console.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace EF_Console.Tests.Services
{
    [TestFixture]
    public class BookServicesIntegrationTests
    {
        private readonly string testConnectionString = ConnectionString.TESTING;
        private BookService _testBookService;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _testBookService = new BookService(testConnectionString);
        }

        [SetUp]
        public void SetUp()
        {
            using (var db = new Context(testConnectionString))
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }
        }

        [Test]
        public void GetBooksCountByAuthor_MustReturnCorrectValue()
        {
            //Arrange
            using (var db = new Context(testConnectionString))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    var author1 = new Author { FirstName = "test1", SecondName = "test1", LastName = "test1", Id = 10 };
                    var author2 = new Author { FirstName = "test2", SecondName = "test2", LastName = "test2", Id = 4 };

                    db.Authors.AddRange(author1, author2);

                    var book1 = new Book { Title = "book1", Year_of_issue = new DateTime(2000, 01, 01) };
                    var book2 = new Book { Title = "book2", Year_of_issue = new DateTime(2000, 01, 01) };
                    var book3 = new Book { Title = "book3", Year_of_issue = new DateTime(2000, 01, 01) };
                    var book4 = new Book { Title = "book4", Year_of_issue = new DateTime(2000, 01, 01) };

                    db.Books.AddRange(book1, book2, book3, book4);

                    author1.Books = new List<Book> { book1, book2, book3 };
                    author2.Books = new List<Book> { book3, book4 };

                    db.Database.ExecuteSqlInterpolated($"set identity_insert dbo.Authors on;");
                    db.SaveChanges();
                    db.Database.ExecuteSqlInterpolated($"set identity_insert dbo.Authors off;");

                    trans.Commit();
                }
            }

            //Act
            int countBookById_1 = _testBookService.GetBooksCountByAuthor(10);
            int countBookById_2 = _testBookService.GetBooksCountByAuthor(4);
            int countBookByFullName_1 = _testBookService.GetBooksCountByAuthor("test1", "test1", "test1");
            int countBookByFullName_2 = _testBookService.GetBooksCountByAuthor("test2", "test2", "test2");

            //Assert
            Assert.AreEqual(3, countBookById_1);
            Assert.AreEqual(2, countBookById_2);
            Assert.AreEqual(3, countBookByFullName_1);
            Assert.AreEqual(2, countBookByFullName_2);
        }

        [Test]
        public void GetBooksByGenreAndDate_MustReturnCorrectListBooks()
        {
            //Arrange
            var book1 = new Book { Title = "book1", Year_of_issue = new DateTime(2000, 01, 01), Id = 1 };
            var book2 = new Book { Title = "book2", Year_of_issue = new DateTime(1900, 01, 01), Id = 2 };
            var book3 = new Book { Title = "book3", Year_of_issue = new DateTime(1800, 01, 01), Id = 3 };
            var book4 = new Book { Title = "book4", Year_of_issue = new DateTime(1700, 01, 01), Id = 4 };
            var book5 = new Book { Title = "book5", Year_of_issue = new DateTime(1600, 01, 01), Id = 5 };
            var book6 = new Book { Title = "book6", Year_of_issue = new DateTime(1500, 01, 01), Id = 6 };

            using (var db = new Context(testConnectionString))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    db.Books.AddRange(book1, book2, book3, book4, book5, book6);

                    var genre1 = new Genre { Genre_name = "test1" };
                    var genre2 = new Genre { Genre_name = "test2" };

                    db.Genres.AddRange(genre1, genre2);

                    genre1.Books = new List<Book> { book1, book2, book4, book6 };
                    genre2.Books = new List<Book> { book3, book5, book6 };

                    db.Database.ExecuteSqlInterpolated($"set identity_insert dbo.Books on;");
                    db.SaveChanges();
                    db.Database.ExecuteSqlInterpolated($"set identity_insert dbo.Books off;");

                    trans.Commit();
                }
            }

            //Act
            var testListGenre1 = _testBookService.GetBooksByGenreAndDate("test1", 1500, 2000);
            var testListGenre2 = _testBookService.GetBooksByGenreAndDate("test2", 1500, 1600);

            //Assert
            Assert.AreEqual(3, testListGenre1.Count);
            CollectionAssert.DoesNotContain(testListGenre1, book1);
            CollectionAssert.Contains(testListGenre1, book2);
            CollectionAssert.Contains(testListGenre1, book4);
            CollectionAssert.Contains(testListGenre1, book6);

            Assert.AreEqual(1, testListGenre2.Count);
            CollectionAssert.DoesNotContain(testListGenre2, book5);
            CollectionAssert.Contains(testListGenre2, book6);
            CollectionAssert.DoesNotContain(testListGenre2, book3);
        }

        [Test]
        public void GetBooksCountByGenre_MustReturnCorrectValue()
        {
            //Arrange
            using (var db = new Context(testConnectionString))
            {
                var book1 = new Book { Title = "book1", Year_of_issue = new DateTime(2000, 01, 01) };
                var book2 = new Book { Title = "book2", Year_of_issue = new DateTime(2000, 01, 01) };
                var book3 = new Book { Title = "book3", Year_of_issue = new DateTime(2000, 01, 01) };
                var book4 = new Book { Title = "book4", Year_of_issue = new DateTime(2000, 01, 01) };
                var book5 = new Book { Title = "book5", Year_of_issue = new DateTime(2000, 01, 01) };
                var book6 = new Book { Title = "book6", Year_of_issue = new DateTime(2000, 01, 01) };

                db.Books.AddRange(book1, book2, book3, book4, book5, book6);

                var genre1 = new Genre { Genre_name = "test1" };
                var genre2 = new Genre { Genre_name = "test2" };

                db.Genres.AddRange(genre1, genre2);

                genre1.Books = new List<Book> { book1, book4, book6 };
                genre2.Books = new List<Book> { book1, book2, book3, book5, book6 };

                db.SaveChanges();
            }

            //Act
            int testBookCount_1 = _testBookService.GetBooksCountByGenre("test1");
            int testBookCount_2 = _testBookService.GetBooksCountByGenre("test2");

            //Assert
            Assert.AreEqual(testBookCount_1, 3);
            Assert.AreEqual(testBookCount_2, 5);
        }

        [Test]
        public void CheckBookByTitleAndAuthor_MustReturnCorrectValue()
        {
            //Arrange
            using (var db = new Context(testConnectionString))
            {
                var author1 = new Author { FirstName = "test1", SecondName = "test1", LastName = "test1" };
                var author2 = new Author { FirstName = "test2", SecondName = "test2", LastName = "test2" };

                db.Authors.AddRange(author1, author2);

                var book1 = new Book { Title = "book1", Year_of_issue = new DateTime(2000, 01, 01) };
                var book2 = new Book { Title = "book2", Year_of_issue = new DateTime(2000, 01, 01) };
                var book3 = new Book { Title = "book3", Year_of_issue = new DateTime(2000, 01, 01) };

                db.Books.AddRange(book1, book2, book3);

                author1.Books = new List<Book> { book1, book3 };
                author2.Books = new List<Book> { book2, book3 };

                db.SaveChanges();
            }

            //Act
            bool testAutor1_Book1 = _testBookService.CheckBookByTitleAndAuthor("book1", "test1", "test1", "test1");
            bool testAutor1_Book2 = _testBookService.CheckBookByTitleAndAuthor("book2", "test1", "test1", "test1");
            bool testAutor1_Book3 = _testBookService.CheckBookByTitleAndAuthor("book3", "test1", "test1", "test1");
            bool testAutor2_Book1 = _testBookService.CheckBookByTitleAndAuthor("book1", "test2", "test2", "test2");
            bool testAutor2_Book2 = _testBookService.CheckBookByTitleAndAuthor("book2", "test2", "test2", "test2");
            bool testAutor2_Book3 = _testBookService.CheckBookByTitleAndAuthor("book3", "test2", "test2", "test2");

            //Assert
            Assert.AreEqual(testAutor1_Book1, true);
            Assert.AreEqual(testAutor1_Book2, false);
            Assert.AreEqual(testAutor1_Book3, true);
            Assert.AreEqual(testAutor2_Book1, false);
            Assert.AreEqual(testAutor2_Book2, true);
            Assert.AreEqual(testAutor2_Book3, true);
        }

        [Test]
        public void CheckUserIsBook_MustReturnCorrectValue()
        {
            //Arrange
            var user1 = new User { Name = "test1", Id = 4 };
            var user2 = new User { Name = "test1", Id = 7 };

            var book1 = new Book { Title = "book1", Year_of_issue = new DateTime(2000, 01, 01), Id = 3 };
            var book2 = new Book { Title = "book2", Year_of_issue = new DateTime(2000, 01, 01), Id = 6 };
            var book3 = new Book { Title = "book3", Year_of_issue = new DateTime(2000, 01, 01), Id = 9 };

            using (var db = new Context(testConnectionString))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    db.Users.AddRange(user1, user2);

                    db.Database.ExecuteSqlInterpolated($"set identity_insert dbo.Users on;");
                    db.SaveChanges();
                    db.Database.ExecuteSqlInterpolated($"set identity_insert dbo.Users off;");

                    trans.Commit();
                }

                using (var trans = db.Database.BeginTransaction())
                {
                    db.Books.AddRange(book1, book2, book3);

                    book1.User = user1;
                    book2.User = user1;
                    book3.User = user2;

                    db.Database.ExecuteSqlInterpolated($"set identity_insert dbo.Books on;");
                    db.SaveChanges();
                    db.Database.ExecuteSqlInterpolated($"set identity_insert dbo.Books off;");

                    trans.Commit();
                }
            }

            //Act
            bool testUser1_Book1 = _testBookService.CheckUserIsBook(user1.Id, book1.Id);
            bool testUser1_Book2 = _testBookService.CheckUserIsBook(user1.Id, book2.Id);
            bool testUser1_Book3 = _testBookService.CheckUserIsBook(user1.Id, book3.Id);
            bool testUser2_Book1 = _testBookService.CheckUserIsBook(user2.Id, book1.Id);
            bool testUser2_Book2 = _testBookService.CheckUserIsBook(user2.Id, book2.Id);
            bool testUser2_Book3 = _testBookService.CheckUserIsBook(user2.Id, book3.Id);

            //Assert
            Assert.AreEqual(testUser1_Book1, true);
            Assert.AreEqual(testUser1_Book2, true);
            Assert.AreEqual(testUser1_Book3, false);
            Assert.AreEqual(testUser2_Book1, false);
            Assert.AreEqual(testUser2_Book2, false);
            Assert.AreEqual(testUser2_Book3, true);
        }

        [Test]
        public void CountBooksOnUser_MustReturnCorrectValue()
        {
            //Arrange
            var user1 = new User { Name = "test1", Id = 4 };
            var user2 = new User { Name = "test1", Id = 7 };

            using (var db = new Context(testConnectionString))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    db.Users.AddRange(user1, user2);

                    var book1 = new Book { Title = "book1", Year_of_issue = new DateTime(2000, 01, 01) };
                    var book2 = new Book { Title = "book2", Year_of_issue = new DateTime(2000, 01, 01) };
                    var book3 = new Book { Title = "book3", Year_of_issue = new DateTime(2000, 01, 01) };
                    var book4 = new Book { Title = "book4", Year_of_issue = new DateTime(2000, 01, 01) };
                    var book5 = new Book { Title = "book5", Year_of_issue = new DateTime(2000, 01, 01) };

                    db.Books.AddRange(book1, book2, book3, book4, book5);

                    user1.Books = new List<Book> { book1, book3, book4, book5 };
                    user2.Books = new List<Book> { book2 };

                    db.Database.ExecuteSqlInterpolated($"set identity_insert dbo.Users on;");
                    db.SaveChanges();
                    db.Database.ExecuteSqlInterpolated($"set identity_insert dbo.Users off;");

                    trans.Commit();
                }
            }

            //Act
            int testCountBook1 = _testBookService.CountBooksOnUser(user1.Id);
            int testCountBook2 = _testBookService.CountBooksOnUser(user2.Id);

            //Assert
            Assert.AreEqual(testCountBook1, 4);
            Assert.AreEqual(testCountBook2, 1);
        }

        [Test]
        public void NewestBook_MustReturnCorrectObject()
        {
            //Arrange
            var book1 = new Book { Title = "book1", Year_of_issue = new DateTime(2000, 01, 01), Id = 1 };
            var book2 = new Book { Title = "book2", Year_of_issue = new DateTime(1900, 01, 01), Id = 2 };
            var book3 = new Book { Title = "book3", Year_of_issue = new DateTime(1800, 01, 01), Id = 3 };
            var book4 = new Book { Title = "book4", Year_of_issue = new DateTime(1700, 01, 01), Id = 4 };
            var book5 = new Book { Title = "book5", Year_of_issue = new DateTime(1600, 01, 01), Id = 5 };
            var book6 = new Book { Title = "book6", Year_of_issue = new DateTime(1500, 01, 01), Id = 6 };

            using (var db = new Context(testConnectionString))
            {
                using(var trans = db.Database.BeginTransaction())
                {
                    db.Books.AddRange(book1, book2, book3, book4, book5, book6);

                    db.Database.ExecuteSqlInterpolated($"set identity_insert dbo.Books on;");
                    db.SaveChanges();
                    db.Database.ExecuteSqlInterpolated($"set identity_insert dbo.Books off;");

                    trans.Commit();
                }
            }

            //Act
            var testNewestBook = _testBookService.NewestBook();

            //Assert
            Assert.AreEqual(testNewestBook, book1);
        }

        [Test]
        public void AllBooksOrderByTitle_MustReturnListByCorrectOrder()
        {
            //Arrenge
            var book1 = new Book { Title = "second", Year_of_issue = new DateTime(1900, 01, 01) };
            var book2 = new Book { Title = "first", Year_of_issue = new DateTime(2000, 01, 01) };
            var book3 = new Book { Title = "threed", Year_of_issue = new DateTime(1800, 01, 01) };

            using (var db = new Context(testConnectionString))
            {
                db.AddRange(book1, book2, book3);

                db.SaveChanges();
            }

            //Act
            var testListBookOrderByTitleAscending = _testBookService.AllBooksOrderByTitle();

            //Assert
            Assert.AreEqual(testListBookOrderByTitleAscending[0].Title, book2.Title);
            Assert.AreEqual(testListBookOrderByTitleAscending[1].Title, book1.Title);
            Assert.AreEqual(testListBookOrderByTitleAscending[2].Title, book3.Title);
        }

        [Test]
        public void AllBookOrderByDiscendingDate_MustReturnListByCorrectOrder()
        {
            //Arrange
            var book1 = new Book { Title = "book1", Year_of_issue = new DateTime(1900, 01, 01) };
            var book2 = new Book { Title = "book2", Year_of_issue = new DateTime(1500, 01, 01) };
            var book3 = new Book { Title = "book3", Year_of_issue = new DateTime(2100, 01, 01) };

            using (var db = new Context(testConnectionString))
            {
                db.AddRange(book1, book2, book3);

                db.SaveChanges();
            }

            //Act
            var testListBookOrderByDateDiscending = _testBookService.AllBookOrderByDiscendingDate();

            //Assert
            Assert.AreEqual(testListBookOrderByDateDiscending[0].Year_of_issue, book3.Year_of_issue);
            Assert.AreEqual(testListBookOrderByDateDiscending[1].Year_of_issue, book1.Year_of_issue);
            Assert.AreEqual(testListBookOrderByDateDiscending[2].Year_of_issue, book2.Year_of_issue);
        }

        [Test]
        public void CheckBookInHand_MustReturnCorrectValue()
        {
            //Arrange
            var book1 = new Book { Title = "book1", Year_of_issue = new DateTime(2000, 01, 01), Id = 4 };
            var book2 = new Book { Title = "book2", Year_of_issue = new DateTime(2000, 01, 01), Id = 6 };
            var book3 = new Book { Title = "book3", Year_of_issue = new DateTime(2000, 01, 01), Id = 10 };

            var user1 = new User { Name = "test", Id = 4 };

            using (var db = new Context(testConnectionString))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    db.Users.Add(user1);

                    db.Database.ExecuteSqlInterpolated($"set identity_insert dbo.Users on;");
                    db.SaveChanges();
                    db.Database.ExecuteSqlInterpolated($"set identity_insert dbo.Users off;");

                    trans.Commit();
                }

                using (var trans = db.Database.BeginTransaction())
                {
                    db.Books.AddRange(book1, book2, book3);

                    book1.User = user1;
                    book2.User = user1;

                    db.Database.ExecuteSqlInterpolated($"set identity_insert dbo.Books on;");
                    db.SaveChanges();
                    db.Database.ExecuteSqlInterpolated($"set identity_insert dbo.Books off;");

                    trans.Commit();
                }                
            }

            //Act
            bool testCheckBookInHand_1 = _testBookService.CheckBookInHand(book1.Id);
            bool testCheckBookInHand_2 = _testBookService.CheckBookInHand(book2.Id);
            bool testCheckBookInHand_3 = _testBookService.CheckBookInHand(book3.Id);

            //Assert
            Assert.AreEqual(testCheckBookInHand_1, true);
            Assert.AreEqual(testCheckBookInHand_2, true);
            Assert.AreEqual(testCheckBookInHand_3, false);
        }
    }
}
