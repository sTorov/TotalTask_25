using EF_Console.Entity;
using EF_Console.Repository;
using NUnit.Framework;

namespace EF_Console.Tests.Repository
{
    [TestFixture]
    public class BookRepositoryIntegrationTests
    {
        private Book testBook;
        private BookRepository bookRepository;

        [OneTimeSetUp]
        public void StartIniitialization()
        {
            bookRepository = new BookRepository();
        }

        [SetUp]
        public void AllTestsInitialization()
        {
            testBook = new Book { Title = "Test", Year_of_issue = new DateTime(2000, 12, 12) };
        }

        [Test]
        public void Add_MustAddingNewBookInBase()
        {
            var bookList = bookRepository.FindAll();

            Assert.IsNotNull(bookList);
            CollectionAssert.DoesNotContain(bookList, testBook);

            bookRepository.Add(testBook);

            bookList = bookRepository.FindAll();
            testBook.Id = bookRepository.GetIdByTitleAndYear(testBook.Title, testBook.Year_of_issue);

            CollectionAssert.Contains(bookList, testBook);

            bookRepository.Delete(testBook);

            bookList = bookRepository.FindAll();

            CollectionAssert.DoesNotContain(bookList, testBook);
        }

        [Test]
        public void FindById_MustFindBookInBase()
        {
            bookRepository.Add(testBook);

            testBook.Id = bookRepository.GetIdByTitleAndYear(testBook.Title, testBook.Year_of_issue);

            var findUser = bookRepository.FindById(testBook.Id);

            Assert.AreEqual(findUser, testBook);

            bookRepository.Delete(testBook);
            findUser = bookRepository.FindById(testBook.Id);

            Assert.IsNull(findUser);
        }

        [Test]
        public void UpdateYearOfIssueById_MustChangeYearOfIssueForBookInBase()
        {
            var newDate = new DateTime(2022, 01, 01);

            bookRepository.Add(testBook);

            testBook.Id = bookRepository.GetIdByTitleAndYear(testBook.Title, testBook.Year_of_issue);

            bookRepository.UpdateYearOfIssueById(testBook.Id, newDate);
            var findBook = bookRepository.FindById(testBook.Id);

            Assert.AreEqual(findBook.Year_of_issue, newDate);

            bookRepository.Delete(findBook);
            findBook = bookRepository.FindById(testBook.Id);

            Assert.IsNull(findBook);
        }
    }
}
