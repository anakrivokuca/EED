using EED.Domain;
using EED.Service.Controller.Contests;
using EED.Ui.Web.Controllers;
using EED.Ui.Web.Models.Contests;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EED.Unit.Tests.Controllers
{
    [TestFixture]
    class ContestControllerTest
    {
        private Mock<IContestServiceController> _mock;
        private ContestController _controller;
        private IEnumerable<Contest> _contests;

        [SetUp]
        public void SetUp_ContestControllerTest()
        {
            // Arrange
            _contests = new List<Contest> { 
                new Contest { Id = 1, Name = "Contest1", 
                    Office = new Office { Id = 1 },
                    District = new District { Id = 1, Name = "District1"}, 
                    Project = new ElectionProject { Id = 1 }},
                new Contest { Id = 2, Name = "Contest2", 
                    Office = new Office { Id = 1 },
                    District = new District { Id = 2, Name = "District2"}, 
                    Project = new ElectionProject { Id = 2 }},
                new Contest { Id = 3, Name = "Contest3", 
                    Office = new Office { Id = 2 },
                    District = new District { Id = 2, Name = "District2"}, 
                    Project = new ElectionProject { Id = 2 }}};

            _mock = new Mock<IContestServiceController>();

            _mock.Setup(p => p.FindProject(1)).Returns(new ElectionProject
            {
                Id = 1,
                Offices = new List<Office> { new Office { Id = 1 }, 
                    new Office { Id = 2, DistrictType = new DistrictType { Id = 1 } } },
                Districts = new List<District> { new District { Id = 1 }, 
                    new District { Id = 2, DistrictType = new DistrictType { Id = 1 } } },
                Contests = _contests.ToList()
            });

            _controller = new ContestController(_mock.Object);

            var session = new Mock<HttpSessionStateBase>();
            var context = new Mock<HttpContextBase>();
            session.SetupGet(s => s["projectId"]).Returns("1");
            context.SetupGet(x => x.Session).Returns(session.Object);
            _controller.ControllerContext = new ControllerContext(context.Object,
                new RouteData(), _controller);
        }

        #region Test List Method
        [Test]
        public void List_GivenThreeContests_ReturnsThreeContests()
        {
            // Act
            var result = ((ListViewModel)_controller.List(null).Model).ContestsPerPage;

            // Assert
            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public void List_GivenThreeContestsAndTwoContestsPerPage_ReturnsOneContestOnTheSecondPage()
        {
            // Arrange
            _controller.ItemsPerPage = 2;

            // Act
            var result = ((ListViewModel)_controller.List(null, 0, 2).Model).ContestsPerPage;

            // Assert
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void List_GetFilteredContests_ReturnsOneContest()
        {
            // Arrange
            var searchText = "Contest1";
            var officeId = 1;
            _mock.Setup(c => c.FilterContests(_contests, searchText, officeId)).Returns(
                new List<Contest> {
                    new Contest { Id = 1, Name = "Contest1" }});

            // Act
            var result = ((ListViewModel)_controller.List(searchText, officeId).Model).ContestsPerPage;

            // Assert
            Assert.AreEqual(1, result.Count());
        }
        #endregion
    }
}
