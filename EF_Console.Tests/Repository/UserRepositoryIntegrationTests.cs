using EF_Console.Entity;
using EF_Console.Repository;
using NUnit.Framework;

namespace EF_Console.Tests.Repository
{
    [TestFixture]
    public class UserRepositoryIntegrationTests
    {
        private User testUser;
        private UserRepository userRepository;

        [OneTimeSetUp]
        public void StartIniitialization()
        {
            userRepository = new UserRepository();
        }

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
            CollectionAssert.IsNotEmpty(userList);
            CollectionAssert.DoesNotContain(userList, testUser);
            
            userRepository.Add(testUser);

            userList = userRepository.FindAll();
            testUser.Id = userList.Where(u => u.Email == testUser.Email && u.Name == testUser.Name)
                .Select(u => u.Id)
                .First();

            CollectionAssert.Contains(userList, testUser);

            userRepository.Delete(testUser);

            userList = userRepository.FindAll();

            CollectionAssert.DoesNotContain(userList, testUser);
        }

        [Test]
        public void FindById_MustFindUserInBase()
        {           
            userRepository.Add(testUser);

            var userList = userRepository.FindAll();
            testUser.Id = userList.Where(u => u.Email == testUser.Email && u.Name == testUser.Name)
                .Select(u => u.Id)
                .First();

            var findUser = userRepository.FindById(testUser.Id);

            Assert.AreEqual(findUser, testUser);

            userRepository.Delete(testUser);
            findUser = userRepository.FindById(testUser.Id);

            Assert.IsNull(findUser);
        }

        [TestCase("newTest")]
        public void UpdateUserNameById_MustChangeUserNameInBase(string newName)
        {
            userRepository.Add(testUser);

            var userList = userRepository.FindAll();
            testUser.Id = userList.Where(u => u.Email == testUser.Email && u.Name == testUser.Name)
                .Select(u => u.Id)
                .First();

            userRepository.UpdateUserNameById(testUser.Id, newName);
            var findUser = userRepository.FindById(testUser.Id);

            Assert.AreEqual(findUser.Name, newName);

            userRepository.Delete(findUser);
            findUser = userRepository.FindById(testUser.Id);

            Assert.IsNull(findUser);
        }
    }
}
