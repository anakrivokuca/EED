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

        #region Test Edit (Get) Method
        [Test]
        public void Edit_GetContest_ReturnsCreateViewModel()
        {
            // Arrange
            var contestId = 1;
            _mock.Setup(s => s.FindContest(contestId)).Returns(new Contest
            {
                Id = contestId,
                Name = "Contest1",
                Office = new Office { Id = 1 },
                District = new District { Id = 2 }
            });

            // Act
            var result = (CreateViewModel)_controller.Edit(contestId).Model;

            // Assert
            Assert.AreEqual("Contest1", result.Name);
        }

        [Test]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void Edit_GetNonexistentContest_ThrowsException()
        {
            // Arrange
            var contestId = 101;
            Contest contest = null;
            _mock.Setup(s => s.FindContest(contestId)).Returns(contest);

            // Act
            var result = (CreateViewModel)_controller.Edit(contestId).Model;

            // Assert
            Assert.IsNull(result);
        }
        #endregion

        #region Test Edit (Post) Method
        [Test]
        public void Edit_PostNewContest_ReturnsRedirectResult()
        {
            // Arrange
            var model = new CreateViewModel
            {
                Name = "NewContest"
            };

            // Act
            var result = _controller.Edit(model);

            // Assert
            _mock.Verify(m => m.SaveContest(It.IsAny<Contest>()), Times.Once());
            Assert.IsNotNull(_controller.TempData["message-success"]);
            Assert.AreEqual("Contest NewContest has been successfully saved.",
                _controller.TempData["message-success"]);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
        }

        [Test]
        public void Edit_PostExistingContestWithValidChanges_ReturnsRedirectResult()
        {
            // Arrange
            var model = new CreateViewModel
            {
                Id = 2,
                Name = "Contest 2"
            };
            var contest = new Contest
            {
                Id = 2,
                Name = "Contest2"
            };
            _mock.Setup(d => d.FindContest(model.Id)).Returns(contest);

            // Act
            var result = _controller.Edit(model);

            // Assert
            _mock.Verify(m => m.SaveContest(It.IsAny<Contest>()));
            Assert.IsNotNull(_controller.TempData["message-success"]);
            Assert.AreEqual("Contest Contest 2 has been successfully saved.",
                _controller.TempData["message-success"]);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
        }
        #endregion

        #region Test Delete Method
        [Test]
        public void Delete_GetValidContest_ReturnsSuccessMessage()
        {
            // Arrange
            var contestId = 1;
            var contest = new Contest
            {
                Id = contestId,
                Name = "Contest1"
            };
            _mock.Setup(s => s.FindContest(contestId)).Returns(contest);

            // Act
            _controller.Delete(contestId);

            // Assert
            _mock.Verify(m => m.DeleteContest(contest), Times.Once());
            Assert.IsNotNull(_controller.TempData["message-success"]);
            Assert.AreEqual("Contest " + contest.Name + " has been successfully deleted.",
                _controller.TempData["message-success"]);
        }

        [Test]
        public void Delete_PostMultipleContests_ReturnsSuccessMessage()
        {
            // Arrange
            var contests = new int[] { 1, 2, 3 };
            _mock.Setup(s => s.FindContest(contests[0])).Returns(new Contest { Id = contests[0] });
            _mock.Setup(s => s.FindContest(contests[1])).Returns(new Contest { Id = contests[1] });
            _mock.Setup(s => s.FindContest(contests[2])).Returns(new Contest { Id = contests[2] });

            // Act
            _controller.Delete(contests);

            // Assert
            _mock.Verify(m => m.DeleteContest(It.IsAny<Contest>()), Times.Exactly(contests.Count()));
            Assert.IsNotNull(_controller.TempData["message-success"]);
            Assert.AreEqual(contests.Count() + " contest(s) has been successfully deleted.",
                _controller.TempData["message-success"]);
        }

        [Test]
        public void Delete_PostNoContest_ReturnsInfoMessage()
        {
            // Arrange
            int[] contests = null;

            // Act
            _controller.Delete(contests);

            // Assert
            _mock.Verify(m => m.DeleteContest(It.IsAny<Contest>()), Times.Never);
            Assert.IsNotNull(_controller.TempData["message-info"]);
            Assert.AreEqual("None of the contests has been selected for delete action.",
                _controller.TempData["message-info"]);
        }
        #endregion
    }
}
