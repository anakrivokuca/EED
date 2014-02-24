using EED.Infrastructure;
using EED.Service.Controller.User;
using EED.Ui.Web.Controllers;
using EED.Ui.Web.Models.User;
using NUnit.Framework;
using System.Linq;
using System.Web.Mvc;
using TechTalk.SpecFlow;

namespace EED.Acceptance.Tests.Steps
{
    [Binding]
    public class AddUserSteps
    {
        private UserController _controller;
        private IUserServiceController _serviceController;
        private CreateViewModel _model;
        private ActionResult _result;

        [When (@"an administrator browses to the Add New User page")]
        [Given(@"I am on the Add New User page")]
        public void WhenAnAdministratorBrowsesToTheAddNewUserPage()
        {
            DependencyResolver.SetResolver(new NinjectDependencyResolver());
            _serviceController = DependencyResolver.Current.GetService<IUserServiceController>();
            _controller = new UserController(_serviceController);
            _result = _controller.Create();
        }

        [Then(@"the Add New User page should be displayed")]
        public void ThenTheUsersPageShouldBeDisplayed()
        {
            Assert.IsInstanceOf<ViewResult>(_result);
            Assert.AreEqual("Edit", ((ViewResult)_result).ViewName);
            Assert.AreEqual("Add New User", _controller.ViewBag.Title);
        }

        [When(@"I enter the valid user data")]
        public void WhenIEnterTheValidUserData()
        {
            _model = new CreateViewModel
            {
                Name = "Jane",
                Surname = "McLaren",
                Email = "jane@nycity.com",
                PhoneNumber = "720-123-5477",
                UserName = "janemclaren",
                Password = "janemclaren123!"
            };
        }

        [When(@"I try to save the user")]
        public void WhenITryToSaveTheUser()
        {
            _result = _controller.Edit(_model);
        }

        [Then(@"I should see a success message")]
        public void ThenIShouldSeeASuccessMessage()
        {
            Assert.IsNotNull(_result);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), _result);
            Assert.AreEqual("User Jane McLaren has been successfully saved.",
                _controller.TempData["message-success"]);
        }

        [Then(@"the user should be listed on the screen")]
        public void ThenTheUserShouldBeListedOnTheScreen()
        {
            var user = _serviceController.GetUser(_model.UserName);
            Assert.IsNotNull(user);
            Assert.IsTrue(user.Id > 0);

        }

        [When(@"the email ""(.*)"" is already taken")]
        public void WhenTheEmailIsAlreadyTaken(string existingEmail)
        {
            var username = _serviceController.GetUserNameByEmail(existingEmail);
            var user = _serviceController.GetUser(username);
            Assert.IsNotNull(user);
            _model.Email = existingEmail;
        }

        [When(@"I enter the password ""(.*)""")]
        public void WhenIEnterThePassword(string password)
        {
            _model.Password = password;
        }

        [Then(@"I should see an error on the screen ""(.*)""")]
        public void ThenIShouldSeeAnErrorOnTheScreen(string message)
        {
            var allErrors = _controller.ModelState.Values.SelectMany(v => v.Errors);
            Assert.AreEqual(1, allErrors.Count(), "One error message should be found.");

            var errorMessage = allErrors.SingleOrDefault(e => e.ErrorMessage == message);
            Assert.IsNotNull(errorMessage, "One error message " + message + " should be found.");
        }
    }
}
