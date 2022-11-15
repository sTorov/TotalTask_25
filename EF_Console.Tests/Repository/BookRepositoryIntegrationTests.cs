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

        private int GetBookId(List<Book> bookList)
        {
            return bookList.Where(b => b.Title == testBook.Title &&
                    b.Year_of_issue == testBook.Year_of_issue)
                .Select(u => u.Id)
                .First();
        }

        [Test]
        public void Add_MustAddingNewBookInBase()
        {
            var bookList = bookRepository.FindAll();

            Assert.IsNotNull(bookList);
            CollectionAssert.IsNotEmpty(bookList);
            CollectionAssert.DoesNotContain(bookList, testBook);

            bookRepository.Add(testBook);

            bookList = bookRepository.FindAll();
            testBook.Id = GetBookId(bookList);

            CollectionAssert.Contains(bookList, testBook);

            bookRepository.Delete(testBook);

            bookList = bookRepository.FindAll();

            CollectionAssert.DoesNotContain(bookList, testBook);
        }

        [Test]
        public void FindById_MustFindBookInBase()
        {
            bookRepository.Add(testBook);

            testBook.Id = GetBookId(bookRepository.FindAll());

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

            testBook.Id = GetBookId(bookRepository.FindAll());

            bookRepository.UpdateYearOfIssueById(testBook.Id, newDate);
            var findBook = bookRepository.FindById(testBook.Id);

            Assert.AreEqual(findBook.Year_of_issue, newDate);

            bookRepository.Delete(findBook);
            findBook = bookRepository.FindById(testBook.Id);

            Assert.IsNull(findBook);
        }
    }
}
