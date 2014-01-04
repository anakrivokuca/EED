using System.Collections.Generic;
using System.Linq;
using EED.DAL;
using EED.Domain;
using EED.Service;
using Moq;
using NUnit.Framework;

namespace EED.Unit.Tests
{
    [TestFixture]
    public class UserServiceTest
    {
        private Mock<IRepository<User>> _mock;
        private UserService _service;

        [SetUp]
        public void Set_Up_UserServiceTest()
        {
            // Arrange
            _mock = new Mock<IRepository<User>>();
            _mock.Setup(r => r.FindAll()).Returns(new List<User> {
                new User { Id = 1, Name = "Ana", Surname = "Krivokuca", 
                    Email = "anakrivokuca@gmail.com"},
                new User { Id = 2, Name = "Pera", Surname = "Peric", 
                    Email = "pera@gmail.com"},
                new User { Id = 3, Name = "John", Surname = "Doe", 
                    Email = "johndoe@gmail.com"},
            });
            _service = new UserService(_mock.Object);
        }
        [Test]
        public void Can_List_All_Users()
        {
            // Act
            var result = _service.FindAllUsers();

            // Assert
            Assert.IsNotNull(result, "Null list is returned.");
            Assert.AreEqual(3, result.Count(), "Number of all users should be three.");
        }

        [Test]
        public void Can_Save_Valid_User()
        {
            // Arrange
            var user = new User
            {
                Id = 4,
                Name = "Sarah"
            };

            // Act
            _service.SaveUser(user);

            // Assert
            _mock.Verify(m => m.Save(user));
        }

        [Test]
        public void Can_Delete_Valid_User()
        {
            // Arrange
            var user = new User { Id = 1 };

            // Act
            _service.DeleteUser(user);

            // Assert
            _mock.Verify(m => m.Delete(user));
        }
    }
}
