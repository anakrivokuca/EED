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
    public class ListAllUsersSteps
    {
        private UserController _controller;
        private IUserServiceController _serviceController;
        private ActionResult _result;

        [When(@"an administrator browses to the Users page")]
        [Given(@"I am on the Users page")]
        public void WhenAnAdministratorBrowsesToTheUsersPage()
        {
            DependencyResolver.SetResolver(new NinjectDependencyResolver());
            _serviceController = DependencyResolver.Current.GetService<IUserServiceController>();
            _controller = new UserController(_serviceController);
            _result = _controller.List(null);
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
            var listedUsers = ((ListViewModel)((ViewResult)_result).Model).Users.ToList();
            Assert.IsTrue(listedUsers.Count() > 0);

            var usersInDatabase = _serviceController.GetAllUsers();
            Assert.AreEqual(usersInDatabase.Count(), listedUsers.Count());
        }
    }
}