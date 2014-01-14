using EED.Domain;
using EED.Service.Membership_Provider;
using EED.Ui.Web.Controllers;
using EED.Ui.Web.Models;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.Web.Mvc;

namespace EED.Unit.Tests.Controllers
{
    [TestFixture]
    class AccountControllerTest
    {
        private Mock<IAuthProvider> _mock;
        //private IEnumerable<User> _users;
        private AccountController _controller;

        [SetUp]
        public void Set_Up_AccountControllerTest()
        {
            // Arrange
            _mock = new Mock<IAuthProvider>();
            
            _controller = new AccountController(_mock.Object);
        }

        [Test]
        public void Login_PostValidCredentials_ReturnsRedirectResult()
        {
            // Arrange
            _mock.Setup(m => m.Authenticate("admin", "admin")).Returns(true);
            var model = new LoginViewModel {
                Username = "admin",
                Password = "admin"
            };

            // Act
            var result = _controller.Login(model, "/url");

            // Assert
            Assert.IsInstanceOf(typeof(RedirectResult), result);
            Assert.AreEqual("/url", ((RedirectResult)result).Url);
        }

        [Test]
        public void Login_PostInvalidCredentials_ReturnsViewResult()
        {
            // Arrange
            _mock.Setup(m => m.Authenticate("badUser", "badPass")).Returns(false);

            var model = new LoginViewModel
            {
                Username = "badUser",
                Password = "badPass"
            };

            // Act
            var result = _controller.Login(model, "/MyURL");

            // Assert
            Assert.IsFalse(_controller.ModelState.IsValid);
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }
    }
}
