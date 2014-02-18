using EED.Domain;
using EED.Service.Controller.User;
using EED.Ui.Web.Controllers;
using EED.Ui.Web.Models.User;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace EED.Unit.Tests.Controllers
{
    [TestFixture]
    class UserControllerTest
    {
        private Mock<IUserServiceController> _mock; 
        private UserController _controller;
        private IEnumerable<User> _users;
        
        [SetUp]
        public void SetUp_UserControllerTest()
        {
            // Arrange
            _users = new List<User> {
                new User { Id = 1, Name = "Ana", Surname = "Krivokuca", 
                    Email = "anakrivokuca@gmail.com", UserName = "anakrivokuca"},
                new User { Id = 2, Name = "Ana", Surname = "Maley", 
                    Email = "ana@gmail.com"},
                new User { Id = 5, Name = "Sarah", State = "US"},
                new User { Id = 3, Name = "John", Surname = "Doe", 
                    Email = "johndoe@gmail.com", State = "US", UserName = "johndoe"},
                new User { Id = 4, Name = "Jane", State = "US"}};

            _mock = new Mock<IUserServiceController>();
            _mock.Setup(p => p.GetAllUsers()).Returns(_users);
            
            _controller = new UserController(_mock.Object)
            {
                ItemsPerPage = 2
            };
        }

        #region Test List Method
        [Test]
        public void List_GivenTwoUsersOnTheFirstPage_ReturnsTwoUsers()
        {
            // Act
            var result = ((ListViewModel)_controller.List(null).Model).Users;

            // Assert
            var users = result.ToList();

            Assert.AreEqual(2, users.Count(),
                "Number of users listed on the first page should be two.");
            Assert.AreEqual("Ana Krivokuca", users[0].Name + " " + users[0].Surname,
                "First user on the first page should be Ana Krivokuca.");
            Assert.AreEqual("Ana Maley", users[1].Name + " " + users[1].Surname,
                "Second user on the first page should be Ana Maley.");
        }

        [Test]
        public void List_GetPagingInfo_ReturnsPagingInfo()
        {
            // Act
            var result = ((ListViewModel)_controller.List(null, 2).Model).PagingInfo;

            // Assert
            Assert.AreEqual(2, result.CurrentPage, 
                "Current page should be two.");
            Assert.AreEqual(2, result.ItemsPerPage, 
                "Items per page should be two.");
            Assert.AreEqual(5, result.TotalNumberOfItems, 
                "Total number of items should be five.");
        }

        [Test]
        public void List_GetTwoUsersOnTheSecondPage_ReturnsTwoUsers()
        {
            // Act
            var result = ((ListViewModel)_controller.List(null, 2).Model).Users;

            // Assert
            var users = result.ToList();

            Assert.AreEqual(2, users.Count, 
                "Number of users on the second page should be two.");
            Assert.AreEqual("John", users[0].Name, 
                "First user on the second page should be John.");
            Assert.AreEqual("Jane", users[1].Name, 
                "Second user on the second page should be Jane.");
        }

        [Test]
        public void List_GetFilteredUsers_ReturnsThreeUsers()
        {
            // Arrange
            var searchText = "US";
            _mock.Setup(p => p.FilterUsers(_users, searchText)).Returns(new List<User> { 
                new User { Id = 5, Name = "Sarah", State = "US"},
                new User { Id = 3, Name = "John", Surname = "Doe", 
                    Email = "johndoe@gmail.com", State = "US"},
                new User { Id = 4, Name = "Jane", State = "US"}});

            // Act
            var result = ((ListViewModel)_controller.List(searchText).Model)
                .PagingInfo.TotalNumberOfItems;

            // Assert
            Assert.AreEqual(3, result, "Total number of items should be three.");
        }
        #endregion

        #region Test Edit (Get) Method
        [Test]
        public void Edit_GetValidUser_ReturnsCreateViewModel()
        {
            // Act
            var result1 = (CreateViewModel)_controller.Edit(1).Model;
            var result2 = (CreateViewModel)_controller.Edit(2).Model;
            var result3 = (CreateViewModel)_controller.Edit(3).Model;

            // Assert
            Assert.AreEqual("Ana Krivokuca", result1.Name + " " + result1.Surname,
                "Selected user should be Ana Krivokuca.");
            Assert.AreEqual("Ana Maley", result2.Name + " " + result2.Surname,
                "Selected user should be Ana Maley.");
            Assert.AreEqual("John Doe", result3.Name + " " + result3.Surname,
                "Selected user should be John Doe.");
        }

        [Test]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void Edit_GetNonexistentUser_ThrowsException()
        {
            // Act
            var result = (CreateViewModel)_controller.Edit(101).Model;

            // Assert
            Assert.IsNull(result);
        }
        #endregion

        #region Test Edit (Post) Method
        [Test]
        public void Edit_PostNewUserWithInvalidPassword_ReturnsViewResult()
        {
            // Arrange
            var model = new CreateViewModel
            {
                Id = 5,
                Name = "Jack",
                Surname = "Doe",
                Email = "jackdoe@ny.com",
                UserName = "jackdoe",
                Password = "badpass"
            };
            var user = model.ConvertModelToUser(model);
            var status = new MembershipCreateStatus();
            status = MembershipCreateStatus.InvalidPassword;
            User returnUser = null;
            _mock.Setup(p => p.CreateUser(user, out status)).Returns(returnUser);

            // Act
            var result = _controller.Edit(model);

            // Assert
            _mock.Verify(m => m.CreateUser(It.IsAny<User>(), out status), Times.Once());
            Assert.IsFalse(_controller.ModelState.IsValid);
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void Edit_PostExistingUserWithValidChanges_ReturnsRedirectResult()
        {
            // Arrange
            var model = new CreateViewModel
            {
                Id = 3,
                Name = "John",
                Surname = "Doe",
                State = "US",
                Country = "Oklahoma",
                UserName = "johndoe"
            };
            var existingUser = _mock.Setup(u => u.GetUser(model.UserName)).Returns(new User { 
                Id = 3, Name = "John", Surname = "Doe", State = "US", UserName = "johndoe"});
            var user = model.ConvertModelToUser(model);

            // Act
            var result = _controller.Edit(model);

            // Assert
            _mock.Verify(m => m.UpdateUser(It.IsAny<User>()));
            Assert.IsNotNull(_controller.TempData["message-success"]);
            Assert.AreEqual("User John Doe has been successfully saved.",
                _controller.TempData["message-success"]); 
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
        }

        [Test]
        public void Edit_PostExistingUserWithInvalidChanges_ReturnsViewResult()
        {
            // Arrange
            var model = new CreateViewModel { Name = "Jack" };
            var user = model.ConvertModelToUser(model);
            _controller.ModelState.AddModelError("error", "error");

            // Act
            var result = _controller.Edit(model);

            // Assert
            _mock.Verify(m => m.UpdateUser(user), Times.Never());
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }
        #endregion

        #region Test Delete Method
        [Test]
        public void Delete_PostValidUser_ReturnsActionResult()
        {
            // Arrange
            var username = "johndoe";
            var user = new User { Id = 1, Name = "John", Surname = "Doe", UserName = username};
            _mock.Setup(p => p.GetUser(username)).Returns(user);
            _mock.Setup(p => p.DeleteUser(username, true)).Returns(true);

            // Act
            _controller.Delete(username);

            // Assert
            _mock.Verify(m => m.DeleteUser(username, true), Times.Once());
            Assert.IsNotNull(_controller.TempData["message-success"]);
            Assert.AreEqual("User " + user.Name + " " +  user.Surname + 
                " has been successfully deleted.",
                _controller.TempData["message-success"]); 
        }
        #endregion
    }
}
