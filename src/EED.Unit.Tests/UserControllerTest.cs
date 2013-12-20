using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EED.Domain;
using EED.Service;
using EED.Ui.Web.Controllers;
using Moq;
using NUnit.Framework;

namespace EED.Unit.Tests
{
    [TestFixture]
    class UserControllerTest
    {
        private Mock<IUserService> _mock;
        private UserController _controller;
        
        [SetUp]
        public void Set_Up_User_ControllerTest()
        {
            // Arrange
            _mock = new Mock<IUserService>();
            _mock.Setup(s => s.FindAllUsers()).Returns(new List<User> {
                new User { Id = 1, Name = "Ana"},
                new User { Id = 2, Name = "Marko"},
                new User { Id = 5, Name = "Sarah"},
                new User { Id = 3, Name = "John"},
                new User { Id = 4, Name = "Jane"}
            });

            _controller = new UserController(_mock.Object) {ItemsPerPage = 2};
        }

        [Test]
        public void Can_Send_Paging_Info_To_Users_View()
        {
            // Act
            var pagingInfo = _controller.Users(2).ViewBag.PagingInfo;

            // Assert
            Assert.AreEqual(2, pagingInfo.CurrentPage);
            Assert.AreEqual(2, pagingInfo.ItemsPerPage);
            Assert.AreEqual(5, pagingInfo.TotalNumberOfItems);
        }

        [Test]
        public void Can_Paginate_Sorted_Users()
        {
            // Act
            var result = (IEnumerable<User>)_controller.Users(2).Model;

            // Assert
            var users = result.ToList();

            Assert.AreEqual(2, users.Count);
            Assert.AreEqual("John", users[0].Name);
            Assert.AreEqual("Jane", users[1].Name);
        }
    }
}
