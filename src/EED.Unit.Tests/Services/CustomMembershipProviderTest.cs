using EED.DAL;
using EED.Domain;
using EED.Infrastructure;
using EED.Service.Membership_Provider;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;

namespace EED.Unit.Tests.Services
{
    [TestFixture]
    public class CustomMembershipProviderTest
    {
        private Mock<IRepository<User>> _mock;
        private IMembershipProvider _provider;
        private IEnumerable<User> _users;

        [SetUp]
        public void SetUp_CustomerMembershipProviderTest()
        {
            // Arrange
            _mock = new Mock<IRepository<User>>();
            _mock.Setup(r => r.FindAll()).Returns(new List<User> {
                new User { Id = 1, Name = "Ana", Surname = "Krivokuca", 
                    Email = "anakrivokuca@gmail.com", Country = "Serbia", UserName = "anakrivokuca",
                    Password = "password", IsApproved = true},
                new User { Id = 2, Name = "Ana", Surname = "Maley", Email = "ana@gmail.com"},
                new User { Id = 5, Name = "Sarah", Email = "sarah@ny.com", 
                    State = "US", Country = "NY"},
                new User { Id = 3, Name = "John", Surname = "Doe", Email = "johndoe@gmail.com", 
                    State = "US", Country = "Oklahoma", UserName = "johndoe",
                    Password = "password", IsApproved = false},
                new User { Id = 4, Name = "Jane", Email = "jane@nyuscom", State = "US"}
            });
            DependencyResolver.SetResolver(new NinjectDependencyResolver());
            _provider = DependencyResolver.Current.GetService<IMembershipProvider>();
            var config = new NameValueCollection();
            _provider.Initialize("", config);
            _provider.Repository = _mock.Object;
        }

        #region Test Initialize Method
        [Test]
        public void Initialize_NullName_SetsDefaultName()
        {
            // Arrange
            _provider = DependencyResolver.Current.GetService<IMembershipProvider>();

            // Act
            _provider.Initialize("", new NameValueCollection());

            // Assert
            Assert.AreEqual("CustomMembershipProvider", _provider.ProviderName);
        }

        [Test]
        public void Initialize_NullRepository_CreatesDefaultRepository()
        {
            // Arrange
            _provider = DependencyResolver.Current.GetService<IMembershipProvider>();
            _provider.Repository = null;

            // Act
            _provider.Initialize("", new NameValueCollection());

            // Assert
            Assert.IsNotNull(_provider.Repository);
        }

        [Test]
        [ExpectedException(typeof(System.Configuration.Provider.ProviderException))]
        public void Initialize_CheckEncryptionKeyFails_ThrowsProviderException()
        {
            // Arrange
            _provider = DependencyResolver.Current.GetService<IMembershipProvider>();
            _provider.MachineKey = _provider.GetMachineKeySection();
            _provider.MachineKey.ValidationKey = "AutoGenerate";
            var config = new NameValueCollection();
            config.Add("passwordFormat", "Hashed");

            // Act
            _provider.Initialize("", config);
        }
        #endregion

        #region Test GetAllUsers Method
        [Test]
        public void GetAllUsers_GivenFiveUsers_ReturnsFiveUsers()
        {
            // Act
            var result = _provider.GetAllUsers();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Count());
        }

        [Test]
        public void GetAllUsers_GivenZeroUsers_ReturnsZeroUsers()
        {
            // Arrange
            _mock.Setup(r => r.FindAll()).Returns(new List<User>());

            // Act
            var result = _provider.GetAllUsers();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }
        #endregion

        #region Test GetUser Method
        [Test]
        public void GetUser_GoodUser_ReturnsUser()
        {
            // Arrange
            var username = "anakrivokuca";

            // Act
            var result = _provider.GetUser(username);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetUser_NonExistentUser_ReturnsNull()
        {
            // Arrange
            var username = "nonExistentUser";

            //Act
            var result = _provider.GetUser(username);

            //Assert
            Assert.IsNull(result);
        }
        #endregion

        #region Test GetUserNameByEmail Method
        [Test]
        public void GetUserNameByEmail_ValidUser_ReturnsUsername()
        {
            // Arrange
            var email = "anakrivokuca@gmail.com";

            // Act
            var result = _provider.GetUserNameByEmail(email);

            // Assert
            Assert.IsNotEmpty(result);
            Assert.AreEqual("anakrivokuca", result);
        }

        [Test]
        public void GetUserNameByEmail_NonexistentUser_ReturnsEmptyUsername()
        {
            // Arrange
            var email = "badEmail";

            // Act
            var result = _provider.GetUserNameByEmail(email);

            // Assert
            Assert.IsEmpty(result);
        }
        #endregion

        #region Test CreateUser Method
        [Test]
        public void CreateUser_ValidUser_ReturnsUser()
        {
            // Arrange
            var user = new User
            {
                Id = 100,
                Name = "NewUser",
                UserName = "newUsername",
                Password = "psw12345!"
            };
            var status = new MembershipCreateStatus();

            // Act
            var result = _provider.CreateUser(user, out status);

            // Assert
            _mock.Verify(m => m.Save(user));
            Assert.AreEqual(MembershipCreateStatus.Success, status);
        }

        [Test]
        public void CreateUser_WithInvalidPassword_ReturnsInvalidPasswordStatus()
        {
            // Arrange
            var user = new User
            {
                Name = "Sarah",
                UserName = "sarah",
                Password = "bad"
            };
            var status = new MembershipCreateStatus();

            // Act
            var result = _provider.CreateUser(user, out status);

            // Assert
            _mock.Verify(m => m.Save(user), Times.Never());
            Assert.AreEqual(MembershipCreateStatus.InvalidPassword, status);
        }

        [Test]
        public void CreateUser_WithDuplicateEmail_ReturnsDuplicateEmailStatus()
        {
            // Arrange
            var user = new User
            {
                Name = "Ana",
                Email = "anakrivokuca@gmail.com",
                UserName = "anak",
                Password = "ana12345!"
            };
            var status = new MembershipCreateStatus();

            // Act
            var result = _provider.CreateUser(user, out status);

            // Assert
            _mock.Verify(m => m.Save(user), Times.Never());
            Assert.AreEqual(MembershipCreateStatus.DuplicateEmail, status);
        }

        [Test]
        public void CreateUser_WithDuplicateUsername_ReturnsDuplicateUsernameStatus()
        {
            // Arrange
            var user = new User
            {
                Name = "Ana",
                Email = "anak@gmail.com",
                UserName = "anakrivokuca",
                Password = "ana12345!"
            };
            var status = new MembershipCreateStatus();

            // Act
            var result = _provider.CreateUser(user, out status);

            // Assert
            _mock.Verify(m => m.Save(user), Times.Never());
            Assert.AreEqual(MembershipCreateStatus.DuplicateUserName, status);
        }
        #endregion

        #region Test UpdateUser Method
        [Test]
        public void UpdateUser_ValidUser_DoesNotThrowError()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Name = "Ana",
                Surname = "Krivokuca",
                Email = "anakrivokuca@gmail.com",
                Country = "Serbia",
                UserName = "anakrivokuca"
            };

            // Act
            _provider.UpdateUser(user);

            // Assert
            _mock.Verify(m => m.Save(It.IsAny<User>()));
        }

        [Test]
        [ExpectedException(typeof(MemberAccessException))]
        public void UpdateUser_NullUser_ThrowsException()
        {
            // Act
            _provider.UpdateUser(null);

            // Assert
            _mock.Verify(m => m.Save(It.IsAny<User>()), Times.Never);
        }
        #endregion

        #region Test DeleteUser Method
        [Test]
        public void DeleteUser_ValidUser_ReturnsTrue()
        {
            // Arrange
            var username = "anakrivokuca";

            // Act
            var result = _provider.DeleteUser(username, true);

            // Assert
            _mock.Verify(m => m.Delete(It.IsAny<User>()));
            Assert.IsTrue(result);
        }

        [Test]
        public void DeleteUser_NonexistentUser_ReturnsFalse()
        {
            // Arrange
            var username = "nonExistentUser";

            // Act
            var result = _provider.DeleteUser(username, true);

            // Assert
            _mock.Verify(m => m.Delete(It.IsAny<User>()), Times.Never);
            Assert.IsFalse(result);
        }
        #endregion

        #region Test ValidateUser Method
        [Test]
        public void ValidateUser_ValidUser_ReturnsTrue()
        {
            // Arrange
            var username = "anakrivokuca";
            var password = "password";

            // Act
            var result = _provider.ValidateUser(username, password);

            // Assert
            var user = _provider.GetUser(username);
            Assert.IsTrue(user.IsApproved);
            Assert.IsTrue(result);
        }

        [Test]
        public void ValidateUser_WrongPassword_ReturnsFalse()
        {
            // Arrange
            var username = "anakrivokuca";
            var password = "badPassword";

            // Act
            var result = _provider.ValidateUser(username, password);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void ValidateUser_NotApprovedUser_ReturnsFalse()
        {
            // Arrange
            var username = "johndoe";
            var password = "password";

            // Act
            var result = _provider.ValidateUser(username, password);

            // Assert
            var user = _provider.GetUser(username);
            Assert.IsFalse(user.IsApproved);
            Assert.IsFalse(result);
        }

        [Test]
        public void ValidateUser_NonexistentUser_ReturnsFalse()
        {
            // Arrange
            var username = "nonExistentUser";
            var password = "nonExistentPass";

            // Act
            var result = _provider.ValidateUser(username, password);

            // Assert
            var user = _provider.GetUser(username);
            Assert.IsNull(user);
            Assert.IsFalse(result);
        }
        #endregion

        #region Test OnValidatePassword Method
        [Test]
        public void OnValidatePassword_ShortPassword_CreatesError()
        {
            // Arrange
            var username = "sarah";
            var password = "shrt1@";
            
            var minRequiredPasswordLength = "7";
            var errorMessage = "[Minimum length: " + minRequiredPasswordLength + "]";

            ValidatePasswordEventArgs args =
                new ValidatePasswordEventArgs(username, password, true);

            // Act
            _provider.OnValidatePassword(_provider, args);

            // Assert
            Assert.IsTrue(args.Cancel);
            Assert.AreEqual(errorMessage, args.FailureInformation.Message);
        }

        [Test]
        public void OnValidatePassword_WeakPassword_CreatesError()
        {
            // Arrange
            var username = "sarah";
            var password = "weakpass@";

            var errorMessage = "[Insufficient Password Strength]";

            ValidatePasswordEventArgs args =
                new ValidatePasswordEventArgs(username, password, true);

            // Act
            _provider.OnValidatePassword(_provider, args);

            // Assert
            Assert.IsTrue(args.Cancel);
            Assert.AreEqual(errorMessage, args.FailureInformation.Message);
        }

        [Test]
        public void OnValidatePassword_NonAlphaChars_CreatesError()
        {
            // Arrange
            var username = "sarah";
            var password = "noalphachars1";

            var errorMessage = "[Insufficient Non-Alpha Characters]";

            ValidatePasswordEventArgs args =
                new ValidatePasswordEventArgs(username, password, true);

            // Act
            _provider.OnValidatePassword(_provider, args);

            // Assert
            Assert.IsTrue(args.Cancel);
            Assert.AreEqual(errorMessage, args.FailureInformation.Message);
        }
        #endregion

        #region Test FilterUsers Method
        [Test]
        public void FilterUsers_ByMultipleCriteria_ReturnsDifferentUsers()
        {
            // Arrange
            _users = _mock.Object.FindAll();

            // Act
            var resultByName = _provider.FilterUsers(_users, "Ana");
            var resultByNameAndSurname = _provider.FilterUsers(_users, "Ana Krivokuca");
            var resultByEmail = _provider.FilterUsers(_users, "anakrivokuca@gmail.com");
            var resultByState = _provider.FilterUsers(_users, "US");
            var resultByStateAndCountry = _provider.FilterUsers(_users, "US Oklahoma");
            var resultByUsername = _provider.FilterUsers(_users, "anakrivokuca");
            var resultByAll = _provider.FilterUsers(_users,
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
            _users = _mock.Object.FindAll();

            // Act
            var resultWithSpaces = _provider.FilterUsers(_users,
                "  John   Doe johndoe@gmail.com   US ");
            var resultWithNonexistentUser = _provider.FilterUsers(_users, "Don");
            var resultWithKeywordsFromDifferentUsers = _provider.FilterUsers(_users, "Ana Krivokuca US");

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
    }
}
