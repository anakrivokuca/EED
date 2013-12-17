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
    public class UserTest
    {
        [Test]
        public void Can_list_all_users()
        {
            // Arrange
            var mock = new Mock<IRepository<User>>();
            mock.Setup(r => r.FindAll()).Returns(new List<User> {
                new User { Name = "Ana", Surname = "Krivokuca", Email = "anakrivokuca@gmail.com"},
                new User { Name = "Pera", Surname = "Peric", Email = "pera@gmail.com"},
                new User { Name = "John", Surname = "Doe", Email = "johndoe@gmail.com"},
            });
            var service = new UserService(mock.Object);
            
            // Act
            var result = service.FindAllUsers();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
        }
    }
}
