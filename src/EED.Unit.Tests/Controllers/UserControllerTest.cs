using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EED.Domain;
using EED.Ui.Web.Controllers;
using EED.Ui.Web.Models;
using Moq;
using NUnit.Framework;
using EED.Service.Membership_Provider;
using System.Web.Security;
using EED.DAL;

namespace EED.Unit.Tests.Controllers
{
    [TestFixture]
    class UserControllerTest
    {
        private Mock<IMembershipProvider> _mock;
        private IEnumerable<User> _users; 
        private UserController _controller;
        
        [SetUp]
        public void SetUp_UserControllerTest()
        {
            // Arrange
            _mock = new Mock<IMembershipProvider>();
            _mock.Setup(p => p.GetAllUsers()).Returns(new List<User> {
                new User { Id = 1, Name = "Ana", Surname = "Krivokuca", 
                    Email = "anakrivokuca@gmail.com", Country = "Serbia", UserName = "anakrivokuca"},
                new User { Id = 2, Name = "Ana", Surname = "Maley", Email = "ana@gmail.com"},
                new User { Id = 5, Name = "Sarah", State = "US", Country = "NY"},
                new User { Id = 3, Name = "John", Surname = "Doe", Email = "johndoe@gmail.com", 
                    State = "US", Country = "Oklahoma", UserName = "johndoe"},
                new User { Id = 4, Name = "Jane", State = "US"}
            });
            
            _controller = new UserController()
            {
                ItemsPerPage = 2,
                _provider = _mock.Object
            };
        }

        #region Test Users Method
        [Test]
        public void Users_GetTwoUsersOnTheFirstPage_ReturnsTwoUsers()
        {
            // Act
            var result = ((UsersListViewModel)_controller.Users(null).Model).Users;

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
        public void Users_GetPagingInfo_ReturnsPagingInfo()
        {
            // Act
            var result = ((UsersListViewModel)_controller.Users(null, 2).Model).PagingInfo;

            // Assert
            Assert.AreEqual(2, result.CurrentPage, 
                "Current page should be two.");
            Assert.AreEqual(2, result.ItemsPerPage, 
                "Items per page should be two.");
            Assert.AreEqual(5, result.TotalNumberOfItems, 
                "Total number of items should be five.");
        }

        [Test]
        public void Users_GetTwoUsersOnTheSecondPage_ReturnsTwoUsers()
        {
            // Act
            var result = ((UsersListViewModel)_controller.Users(null, 2).Model).Users;

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
        public void Users_GetFilteredUsers_ReturnsThreeUsers()
        {
            // Act
            var result = ((UsersListViewModel)_controller.Users("US").Model)
                .PagingInfo.TotalNumberOfItems;

            // Assert
            Assert.AreEqual(3, result, "Total number of items should be three.");
        }
        #endregion

        #region Test FilterUsers Method
        [Test]
        public void FilterUsers_ByMultipleCriteria_ReturnsDifferentUsers()
        {
            // Arrange
            _users = _mock.Object.GetAllUsers();

            // Act
            var resultByName = _controller.FilterUsers(_users, "Ana");
            var resultByNameAndSurname = _controller.FilterUsers(_users, "Ana Krivokuca");
            var resultByEmail = _controller.FilterUsers(_users, "anakrivokuca@gmail.com");
            var resultByState = _controller.FilterUsers(_users, "US");
            var resultByStateAndCountry = _controller.FilterUsers(_users, "US Oklahoma");
            var resultByUsername = _controller.FilterUsers(_users, "anakrivokuca");
            var resultByAll = _controller.FilterUsers(_users, 
                "John Doe johndoe@gmail.com US Oklahoma johndoe");

            // Assert
            var users = resultByName.ToList();
            Assert.AreEqual(2, users.Count, 
                "Two users should be listed with the same name.");
            Assert.AreEqual("Ana Krivokuca", users[0].Name + " " + users[0].Surname,
                "First user with specified name should be Ana Krivokuca.");
            Assert.AreEqual("Ana Maley", users[1].Name + " " + users[1].Surname,
                "Second user with specified name should be Ana Maley.");

            users = resultByNameAndSurname.ToList();
            Assert.AreEqual(1, users.Count,
                "One user should be listed with specified name and surname.");
            Assert.AreEqual("Ana Krivokuca", users[0].Name + " " + users[0].Surname,
                "User with specified name ana surname should be Ana Krivokuca.");

            users = resultByEmail.ToList();
            Assert.AreEqual(1, users.Count,
                "One user should be listed with specified email.");
            Assert.AreEqual("Ana Krivokuca", users[0].Name + " " + users[0].Surname,
                "User with specified email should be Ana Krivokuca.");

            users = resultByState.ToList();
            Assert.AreEqual(3, users.Count,
                "Three users should be listed with the same state.");
            Assert.AreEqual("Sarah", users[0].Name,
                "First user with specified state should be Sarah.");
            Assert.AreEqual("John", users[1].Name,
                "Second user with specified state should be John.");
            Assert.AreEqual("Jane", users[2].Name,
                "Third user with specified state should be Jane.");

            users = resultByStateAndCountry.ToList();
            Assert.AreEqual(1, users.Count,
                "One user should be listed with specified state and country.");
            Assert.AreEqual("John", users[0].Name,
                "User with specified state and country should be John.");

            users = resultByUsername.ToList();
            Assert.AreEqual(1, users.Count,
                 "One user should be listed with specified state and country.");
            Assert.AreEqual("Ana Krivokuca", users[0].Name + " " + users[0].Surname,
                "User with specified email should be Ana Krivokuca.");

            users = resultByAll.ToList();
            Assert.AreEqual(1, users.Count,
                "One user should be listed with specified criteria.");
            Assert.AreEqual("John", users[0].Name,
                "User with all criteria specified should be John.");
        }

        [Test]
        public void FilterUsers_ByIncorrectValues_ReturnsUsersWithoutError()
        {
            // Arrange
            _users = _mock.Object.GetAllUsers();

            // Act
            var resultWithSpaces = _controller.FilterUsers(_users, 
                "  John   Doe johndoe@gmail.com   US ");
            var resultWithNonexistentUser = _controller.FilterUsers(_users, "Don");
            var resultWithKeywordsFromDifferentUsers = _controller.FilterUsers(_users, "Ana Krivokuca US");

            // Assert
            var users = resultWithSpaces.ToList();
            Assert.AreEqual(1, users.Count,
                "One user should be listed with specified criteria.");
            Assert.AreEqual("John", users[0].Name,
                "User with all criteria specified should be John.");

            users = resultWithNonexistentUser.ToList();
            Assert.AreEqual(0, users.Count,
                "No user should be listed with specified criteria.");

            users = resultWithKeywordsFromDifferentUsers.ToList();
            Assert.AreEqual(0, users.Count,
                "No user should be listed with specified criteria.");
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
            //_mock.Verify(m => m.CreateUser(user, out status), Times.Once());
            Assert.IsFalse(_controller.ModelState.IsValid);
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void Edit_PostValidUser_ReturnsRedirectResult()
        {
            // Arrange
            var model = new CreateViewModel
            {
                Id = 3,
                Name = "John",
                Surname = "Doe",
                UserName = "johndoe"
            };
            var user = model.ConvertModelToUser(model);
            _mock.Setup(p => p.UpdateUser(user));

            // Act
            var result = _controller.Edit(model);

            // Assert
            //_mock.Verify(m => m.UpdateUser(user));
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
        }

        [Test]
        public void Edit_PostInvalidUser_ReturnsViewResult()
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
            _mock.Setup(p => p.DeleteUser(username, true)).Returns(true);

            // Act
            _controller.Delete(3, "John", "Doe", username);

            // Assert
            _mock.Verify(m => m.DeleteUser(username, true), Times.Once());
        }
        #endregion
    }
}
