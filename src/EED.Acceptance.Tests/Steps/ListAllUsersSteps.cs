using System;
using System.Web.Mvc;
using EED.DAL;
using EED.Domain;
using EED.Ui.Web.Controllers;
using NUnit.Framework;
using TechTalk.SpecFlow;
using EED.Service.Membership_Provider;
using EED.Infrastructure;

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
            //DependencyResolver.SetResolver(new NinjectDependencyResolver());
            _controller = new UserController();
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
