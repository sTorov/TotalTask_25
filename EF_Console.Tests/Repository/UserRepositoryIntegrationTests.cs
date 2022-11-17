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
            CollectionAssert.DoesNotContain(userList, testUser);
            
            userRepository.Add(testUser);

            userList = userRepository.FindAll();
            testUser.Id = userRepository.GetIdByEmailAndName(testUser.Name, testUser.Email);

            CollectionAssert.Contains(userList, testUser);

            userRepository.Delete(testUser);

            userList = userRepository.FindAll();

            CollectionAssert.DoesNotContain(userList, testUser);
        }

        [Test]
        public void FindById_MustFindUserInBase()
        {           
            userRepository.Add(testUser);

            testUser.Id = userRepository.GetIdByEmailAndName(testUser.Name, testUser.Email); 

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

            testUser.Id = userRepository.GetIdByEmailAndName(testUser.Name, testUser.Email);

            userRepository.UpdateUserNameById(testUser.Id, newName);
            var findUser = userRepository.FindById(testUser.Id);

            Assert.AreEqual(findUser.Name, newName);

            userRepository.Delete(findUser);
            findUser = userRepository.FindById(testUser.Id);

            Assert.IsNull(findUser);
        }
    }
}
