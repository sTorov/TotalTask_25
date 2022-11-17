using EF_Console.Configuration;
using EF_Console.Repository;
using NUnit.Framework;

namespace EF_Console.Tests.Services
{
    [TestFixture]
    public class BookServicesIntegrationTests
    {
        private readonly string testConnectionString = ConnectionString.TESTING;

        private BookRepository _testBookRepository;

        [OneTimeSetUp]
        public void SetUp()
        {
            _testBookRepository = new BookRepository(testConnectionString);
            using (var db = new Context(testConnectionString))
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }
        }

        [Test]
        public void Test()
        {
            
        }
    }
}
