using EED.Infrastructure;
using EED.Service.Controller.User;
using EED.Ui.Web.Controllers;
using NUnit.Framework;
using System.Web.Mvc;
using TechTalk.SpecFlow;

namespace EED.Acceptance.Tests.Steps
{
    [Binding]
    public class ListAllUsersSteps
    {
        private UserController _controller;
        private ActionResult _result;

        [When(@"an administrator browses to the Users page")]
        public void WhenAnAdministratorBrowsesToTheUsersPage()
        {
            DependencyResolver.SetResolver(new NinjectDependencyResolver());
            var serviceController = DependencyResolver.Current.GetService<IUserServiceController>();
            _controller = new UserController(serviceController);
            _result = _controller.List(null);
        }

        [Then(@"the Users page should be displayed")]
        public void ThenTheUsersPageShouldBeDisplayed()
        {
            Assert.IsInstanceOf<ViewResult>(_result);
            Assert.IsEmpty(((ViewResult)_result).ViewName);
            Assert.AreEqual("Users",
                   _controller.ViewBag.Title,
                   "Page title is wrong. Expected to be at the Users page");
        }
    }
}
