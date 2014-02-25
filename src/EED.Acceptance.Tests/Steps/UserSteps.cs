using EED.Acceptance.Tests.ControllerObjects;
using EED.Acceptance.Tests.Utils;
using EED.Domain;
using EED.Service.Controller.User;
using EED.Ui.Web.Controllers;
using EED.Ui.Web.Models.User;
using NUnit.Framework;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using TechTalk.SpecFlow;

namespace EED.Acceptance.Tests.Steps
{
    [Binding]
    public class UserSteps
    {
        private UserController _controller = UserControllerObject.Controller;
        private CreateViewModel _model;
        private ActionResult _result;

        [When(@"an administrator browses to the Users page")]
        [Given(@"I am on the Users page")]
        public void WhenAnAdministratorBrowsesToTheUsersPage()
        {
            _result = _controller.List(null);
        }

        [When(@"an administrator browses to the Add New User page")]
        [Given(@"I am on the Add New User page")]
        public void WhenAnAdministratorBrowsesToTheAddNewUserPage()
        {
            _result = _controller.Create();
        }

        [Given(@"there are more than (.*) users")]
        public void GivenThereAreMoreThanUsers(int itemsPerPage)
        {
            var users = DatabaseHelper.FindAll<User>();
            Assert.IsTrue(users.Count() > itemsPerPage);
        }

        [Given(@"the user with ""(.*)"" username exists")]
        public void GivenTheUserWithUsernameExists(string username)
        {
            var user = DatabaseHelper.FindAll<User>().SingleOrDefault(u => u.UserName == username);
            Assert.IsNotNull(user);
        }

        [Given(@"the user with ""(.*)"" username does not exists")]
        public void GivenTheUserWithUsernameDoesNotExists(string username)
        {
            var user = DatabaseHelper.FindAll<User>().SingleOrDefault(u => u.UserName == username);
            Assert.IsNull(user);
        }

        [When(@"I enter criteria ""(.*)""")]
        public void WhenIEnterCriteria(string criteria)
        {
            _result = _controller.List(criteria);
        }

        [When(@"I enter the valid user data:")]
        public void WhenIEnterTheValidUserData(Table table)
        {
            _model = new CreateViewModel
            {
                Name = table.Rows[0]["Name"],
                Surname = table.Rows[0]["Surname"],
                Email = table.Rows[0]["Email"],
                UserName = table.Rows[0]["UserName"],
                Password = table.Rows[0]["Password"]
            };
        }

        [When(@"I try to save the user")]
        public void WhenITryToSaveTheUser()
        {
            _result = _controller.Edit(_model);
        }

        [When(@"the email ""(.*)"" is already taken")]
        public void WhenTheEmailIsAlreadyTaken(string existingEmail)
        {
            var user = DatabaseHelper.FindAll<User>().SingleOrDefault(u => u.Email == existingEmail);
            Assert.IsNotNull(user);
        }

        [When(@"I enter the invalid password ""(.*)""")]
        public void WhenIEnterTheIvalidPassword(string password)
        {
            _model.Password = password;
        }

        [When(@"I select the valid user:")]
        public void WhenISelectTheValidUser(Table table)
        {
            var user = new User 
            { 
                Name = table.Rows[0]["Name"],
                Surname = table.Rows[0]["Surname"],
                Email = table.Rows[0]["Email"],
                UserName = table.Rows[0]["UserName"],
                Password = table.Rows[0]["Password"]
            };
            DatabaseHelper.SaveOrUpdate(user);

            _model = new CreateViewModel();
            _model.UserName = user.UserName;
        }

        [When(@"I try to delete the user")]
        public void WhenITryToDeleteTheUser()
        {
            _result = _controller.Delete(_model.UserName);
        }

        [Then(@"the Users page should be displayed")]
        public void ThenTheUsersPageShouldBeDisplayed()
        {
            Assert.IsInstanceOf<ViewResult>(_result);
            Assert.AreEqual("Users", _controller.ViewBag.Title);
        }

        [Then(@"all users from the database should be listed")]
        public void ThenAllUsersFromTheDatabaseShouldBeListed()
        {
            var numberofUsers = ((ListViewModel)((ViewResult)_result).Model).PagingInfo.TotalNumberOfItems;
            Assert.IsTrue(numberofUsers > 0);

            var usersInDatabase = DatabaseHelper.FindAll<User>();
            Assert.AreEqual(usersInDatabase.Count(), numberofUsers);
        }

        [Then(@"only (.*) users are listed on the screen")]
        public void ThenOnlyUsersAreListedOnTheScreen(int itemsPerPage)
        {
            var listedUsers = ((ListViewModel)((ViewResult)_result).Model).Users.ToList();
            Assert.IsTrue(listedUsers.Count() > 0);
            Assert.AreEqual(itemsPerPage, listedUsers.Count());
        }

        [Then(@"the user ""(.*)"" should be listed on the screen")]
        public void ThenTheUserShouldBeListedOnTheScreen(string username)
        {
            var listedUsers = ((ListViewModel)((ViewResult)_result).Model).Users.ToList();
            Assert.AreEqual(1, listedUsers.Count);
            Assert.AreEqual("janesmith", listedUsers.First().UserName);
        }

        [Then(@"no user should not be listed on the screen")]
        public void ThenNoUserShouldNotBeListedOnTheScreen()
        {
            var listedUsers = ((ListViewModel)((ViewResult)_result).Model).Users.ToList();
            Assert.AreEqual(0, listedUsers.Count);
        }

        [Then(@"the Add New User page should be displayed")]
        public void ThenTheAddNewUserPageShouldBeDisplayed()
        {
            Assert.IsInstanceOf<ViewResult>(_result);
            Assert.AreEqual("Edit", ((ViewResult)_result).ViewName);
            Assert.AreEqual("Add New User", _controller.ViewBag.Title);
        }

        [Then(@"I should see a success message ""(.*)""")]
        public void ThenIShouldSeeASuccessMessage(string message)
        {
            Assert.IsNotNull(_result);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), _result);
            Assert.AreEqual(message, _controller.TempData["message-success"]);
        }

        [Then(@"the user ""(.*)"" should be saved in the database")]
        public void ThenTheUserShouldBeSavedInTheDatabase(string username)
        {
            var user = DatabaseHelper.FindAll<User>().SingleOrDefault(u => u.UserName == username);
            Assert.IsNotNull(user);
            Assert.IsTrue(user.Id > 0);

            DatabaseHelper.Delete(user);
        }

        [Then(@"I should see an error on the screen ""(.*)""")]
        public void ThenIShouldSeeAnErrorOnTheScreen(string message)
        {
            var allErrors = _controller.ModelState.Values.SelectMany(v => v.Errors);
            Assert.AreEqual(1, allErrors.Count(), "No error was found.");

            var errorMessage = allErrors.SingleOrDefault(e => e.ErrorMessage == message);
            Assert.IsNotNull(errorMessage, "One error message " + message + " should be found.");
        }

        [Then(@"the user ""(.*)"" should not be saved in the database")]
        public void ThenTheUserShouldNotBeSavedInTheDatabase(string username)
        {
            var user = DatabaseHelper.FindAll<User>().SingleOrDefault(u => u.UserName == username);
            Assert.IsNull(user);
        }
    }
}
