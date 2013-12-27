using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EED.Domain;
using EED.Service;
using EED.Ui.Web.Controllers;
using EED.Ui.Web.Models;
using Moq;
using NUnit.Framework;

namespace EED.Unit.Tests
{
    [TestFixture]
    class UserControllerTest
    {
        private Mock<IUserService> _mock;
        private IEnumerable<User> _users; 
        private UserController _controller;
        
        [SetUp]
        public void Set_Up_UserControllerTest()
        {
            // Arrange
            _mock = new Mock<IUserService>();
            _mock.Setup(s => s.FindAllUsers()).Returns(new List<User> {
                new User { Id = 1, Name = "Ana", Surname = "Krivokuca", 
                    Email = "anakrivokuca@gmail.com", Country = "Serbia", Username = "anakrivokuca"},
                new User { Id = 2, Name = "Ana", Surname = "Maley", Email = "ana@gmail.com"},
                new User { Id = 5, Name = "Sarah", State = "US", Country = "NY"},
                new User { Id = 3, Name = "John", Surname = "Doe", Email = "johndoe@gmail.com", 
                    State = "US", Country = "Oklahoma", Username = "johndoe"},
                new User { Id = 4, Name = "Jane", State = "US"}
            });
            _users = _mock.Object.FindAllUsers();
            _controller = new UserController(_mock.Object) {ItemsPerPage = 2};
        }

        [Test]
        public void Can_List_Users_On_First_Page()
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
        public void Can_Send_Paging_Info_To_Users_View()
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
        public void Can_Paginate_Sorted_Users()
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
        public void Can_Calculate_Total_Number_Of_Filtered_Items()
        {
            // Act
            var result = ((UsersListViewModel)_controller.Users("US").Model).PagingInfo;

            // Assert
            Assert.AreEqual(3, result.TotalNumberOfItems,
                "Total number of items should be three.");
        }

        [Test]
        public void Can_Filter_Users_By_Multiple_Criteria()
        {
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
        public void Can_Filter_Users_By_Incorrect_Values()
        {
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

        [Test]
        public void Can_Edit_User()
        {
            // Act
            var result1 = (User)_controller.Edit(1).Model;
            var result2 = (User)_controller.Edit(2).Model;
            var result3 = (User)_controller.Edit(3).Model;

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
        public void Cannot_Edit_Nonexistent_User()
        {
            // Act
            var result = (User)_controller.Edit(101).Model;

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void Can_Save_User_With_Valid_Changes()
        {
            // Arrange
            var user = new User { Name = "Jack" };

            // Act
            var result = _controller.Edit(user);

            // Assert
            _mock.Verify(m => m.SaveUser(user));
            Assert.IsNotInstanceOf(typeof(ViewResult), result);
        }

        [Test]
        public void Cannot_Save_User_With_Invalid_Changes()
        {
            // Arrange
            var user = new User { Name = "Jack" };
            _controller.ModelState.AddModelError("error", "error");

            // Act
            var result = _controller.Edit(user);

            // Assert
            _mock.Verify(m => m.SaveUser(It.IsAny<User>()), Times.Never());
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }
    }
}
