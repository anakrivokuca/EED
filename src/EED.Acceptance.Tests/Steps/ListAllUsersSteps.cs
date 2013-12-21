using System;
using System.Web.Mvc;
using EED.DAL;
using EED.Domain;
using EED.Service;
using EED.Ui.Web.Controllers;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace EED.Acceptance.Tests.Steps
{
    [Binding]
    public class ListAllUsersSteps
    {
        private IRepository<User> _repository;
        private IUserService _service;
        private UserController _controller;
        private ActionResult _result;

        [When(@"an administrator browses to the Users page")]
        public void WhenAnAdministratorBrowsesToTheUsersPage()
        {   
            _repository = new Repository<User>();
            _service = new UserService(_repository);
            _controller = new UserController(_service);
            _result = _controller.Users();
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
