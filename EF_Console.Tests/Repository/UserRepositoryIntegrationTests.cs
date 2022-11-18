using EF_Console.Configuration;
using EF_Console.Entity;
using EF_Console.Repository;
using NUnit.Framework;

namespace EF_Console.Tests.Repository
{
    [TestFixture]
    public class UserRepositoryIntegrationTests
    {
        private readonly string testConnectionString = ConnectionString.TESTING;
        private UserRepository userRepository;

        [OneTimeSetUp]
        public void StartIniitialization()
        {
            userRepository = new UserRepository(testConnectionString);
            using(var db = new Context(testConnectionString))
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }
        }

        
        private User testUser;

        [SetUp]
        public void AllTestsInitialization()
        {
            testUser = new User { Name = "Test", Email = "test@email.com" };
        }

        [Test]
        public void Add_MustAddingNewUserInBase()
        {
            var userList = userRepository.FindAll();

            Assert.IsNotNull(userList);
            CollectionAssert.DoesNotContain(userList, testUser);

            testUser.Id = userRepository.Add(testUser);

            userList = userRepository.FindAll();
            CollectionAssert.Contains(userList, testUser);

            userRepository.Delete(testUser);

            userList = userRepository.FindAll();
            CollectionAssert.DoesNotContain(userList, testUser);
        }

        [Test]
        public void FindById_MustFindUserInBase()
        {
            testUser.Id = userRepository.Add(testUser);

            var findUser = userRepository.FindById(testUser.Id);
            Assert.AreEqual(findUser, testUser);

            userRepository.Delete(testUser);

            findUser = userRepository.FindById(testUser.Id);
            Assert.IsNull(findUser);
        }

        [TestCase("newTest")]
        public void UpdateUserNameById_MustChangeUserNameInBase(string newName)
        {
            testUser.Id = userRepository.Add(testUser);

            userRepository.UpdateUserNameById(testUser.Id, newName);

            var findUser = userRepository.FindById(testUser.Id);
            Assert.AreEqual(findUser.Name, newName);

            userRepository.Delete(findUser);

            findUser = userRepository.FindById(testUser.Id);
            Assert.IsNull(findUser);
        }
    }
}
