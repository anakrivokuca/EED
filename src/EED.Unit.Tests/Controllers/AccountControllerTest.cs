using EED.Service.Controller.Membership_Provider;
using EED.Ui.Web;
using EED.Ui.Web.Controllers;
using EED.Ui.Web.Models.User;
using Moq;
using NUnit.Framework;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EED.Unit.Tests.Controllers
{
    [TestFixture]
    class AccountControllerTest
    {
        private Mock<IAccountServiceController> _mock;
        private AccountController _controller;
        private RouteCollection _routes;
        private Mock<HttpContextBase> _context;

        [SetUp]
        public void Set_Up_AccountControllerTest()
        {
            // Arrange
            _mock = new Mock<IAccountServiceController>();
            
            _controller = new AccountController(_mock.Object);

            _routes = new RouteCollection();
            RouteConfig.RegisterRoutes(_routes);

            var request = new Mock<HttpRequestBase>();
            //request.SetupGet(x => x.ApplicationPath).Returns("/");
            request.SetupGet(x => x.Url).Returns(new Uri("/Account/Login", UriKind.Relative));
            //request.SetupGet(x => x.ServerVariables).Returns(new NameValueCollection());

            var response = new Mock<HttpResponseBase>();
            response.Setup(x => x.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(x => x);

            _context = new Mock<HttpContextBase>();
            _context.SetupGet(x => x.Request).Returns(request.Object);
            _context.SetupGet(x => x.Response).Returns(response.Object);

            _controller.ControllerContext = new ControllerContext(_context.Object, 
                new RouteData(), _controller);
        }

        [Test]
        public void Login_PostValidCredentialsWithUrl_ReturnsRedirectResult()
        {
            // Arrange
            var model = new LoginViewModel {
                Username = "validUser",
                Password = "validPass"
            };
            _mock.Setup(m => m.Authenticate(model.Username, model.Password)).Returns(true);

            // Act
            var result = _controller.Login(model, "/url");

            // Assert
            Assert.IsInstanceOf(typeof(RedirectResult), result);
            Assert.AreEqual("/url", ((RedirectResult)result).Url);
        }

        [Test]
        public void Login_PostValidCredentialsWithoutReturnUrl_ReturnsRedirectResult()
        {
            // Arrange
            var model = new LoginViewModel
            {
                Username = "validUser",
                Password = "validPass"
            };
            _mock.Setup(m => m.Authenticate(model.Username, model.Password)).Returns(true);
            _controller.Url = new UrlHelper(new RequestContext(_context.Object, 
                new RouteData()), _routes);

            // Act
            var result = _controller.Login(model, null);

            // Assert
            Assert.IsInstanceOf(typeof(RedirectResult), result);
            Assert.AreEqual("/", ((RedirectResult)result).Url);
        }

        [Test]
        public void Login_PostAdminCredentialsWithoutReturnUrl_ReturnsRedirectResult()
        {
            // Arrange
            var model = new LoginViewModel
            {
                Username = "admin",
                Password = "admin"
            };
            _mock.Setup(m => m.Authenticate(model.Username, model.Password)).Returns(true);
            _controller.Url = new UrlHelper(new RequestContext(_context.Object, 
                new RouteData()), _routes);

            // Act
            var result = _controller.Login(model, null);

            // Assert
            Assert.IsInstanceOf(typeof(RedirectResult), result);
            Assert.AreEqual("/User", ((RedirectResult)result).Url);
        }

        [Test]
        public void Login_PostInvalidCredentials_ReturnsViewResult()
        {
            // Arrange
            var model = new LoginViewModel
            {
                Username = "badUser",
                Password = "badPass"
            };
            _mock.Setup(m => m.Authenticate(model.Username, model.Password)).Returns(false);

            // Act
            var result = _controller.Login(model, "/url");

            // Assert
            Assert.IsFalse(_controller.ModelState.IsValid);
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }
    }
}
