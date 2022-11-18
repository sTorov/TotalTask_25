using EF_Console.Configuration;
using EF_Console.Entity;
using EF_Console.Repository;
using NUnit.Framework;

namespace EF_Console.Tests.Repository
{
    [TestFixture]
    public class BookRepositoryIntegrationTests
    {
        private readonly string testConnectionString = ConnectionString.TESTING;
        private BookRepository bookRepository;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            bookRepository = new BookRepository(testConnectionString);
            using (var db = new Context(testConnectionString))
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }
        }


        private Book testBook;

        [SetUp]
        public void SetUp()
        {
            testBook = new Book { Title = "Test", Year_of_issue = new DateTime(2000, 12, 12) };
        }

        [Test]
        public void Add_MustAddingNewBookInBase()
        {
            var bookList = bookRepository.FindAll();

            Assert.IsNotNull(bookList);
            CollectionAssert.DoesNotContain(bookList, testBook);

            testBook.Id = bookRepository.Add(testBook);

            bookList = bookRepository.FindAll();
            CollectionAssert.Contains(bookList, testBook);

            bookRepository.Delete(testBook);

            bookList = bookRepository.FindAll();
            CollectionAssert.DoesNotContain(bookList, testBook);
        }

        [Test]
        public void FindById_MustFindBookInBase()
        {
            testBook.Id = bookRepository.Add(testBook);

            var findBook = bookRepository.FindById(testBook.Id);
            Assert.AreEqual(findBook, testBook);

            bookRepository.Delete(testBook);

            findBook = bookRepository.FindById(testBook.Id);
            Assert.IsNull(findBook);
        }

        [Test]
        public void UpdateYearOfIssueById_MustChangeYearOfIssueForBookInBase()
        {
            var newDate = new DateTime(2022, 01, 01);

            testBook.Id = bookRepository.Add(testBook);
            bookRepository.UpdateYearOfIssueById(testBook.Id, newDate);

            var findBook = bookRepository.FindById(testBook.Id);
            Assert.AreEqual(findBook.Year_of_issue, newDate);

            bookRepository.Delete(findBook);

            findBook = bookRepository.FindById(testBook.Id);
            Assert.IsNull(findBook);
        }
    }
}
