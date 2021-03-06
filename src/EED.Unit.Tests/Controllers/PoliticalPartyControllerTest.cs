﻿using EED.Domain;
using EED.Service.Controller.Political_Party;
using EED.Ui.Web.Controllers;
using EED.Ui.Web.Models.Political_Party;
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
    class PoliticalPartyControllerTest
    {
        private Mock<IPoliticalPartyServiceController> _mock;
        private PoliticalPartyController _controller;
        private IEnumerable<PoliticalParty> _politicalParties;

        [SetUp]
        public void SetUp_PoliticalPartyControllerTest()
        {
            // Arrange
            _politicalParties = new List<PoliticalParty> { 
                new PoliticalParty { Id = 1, Name = "PoliticalParty1", 
                    Abbreviation = "PP1", 
                    Project = new ElectionProject { Id = 1 }},
                new PoliticalParty { Id = 2, Name = "PoliticalParty2", 
                    Abbreviation = "PP2", 
                    Project = new ElectionProject { Id = 1 }},
                new PoliticalParty { Id = 3, Name = "PoliticalParty3", 
                    Abbreviation = "PP2", 
                    Project = new ElectionProject { Id = 1 }}};

            _mock = new Mock<IPoliticalPartyServiceController>();

            _mock.Setup(p => p.FindProject(1)).Returns(new ElectionProject
            {
                Id = 1,
                PoliticalParties = _politicalParties.ToList()
            });

            _controller = new PoliticalPartyController(_mock.Object);

            var session = new Mock<HttpSessionStateBase>();
            var context = new Mock<HttpContextBase>();
            session.SetupGet(s => s["projectId"]).Returns("1");
            context.SetupGet(x => x.Session).Returns(session.Object);
            _controller.ControllerContext = new ControllerContext(context.Object,
                new RouteData(), _controller);
        }

        #region Test List Method
        [Test]
        public void List_GivenThreePoliticalParties_ReturnsThreePoliticalParties()
        {
            // Act
            var result = ((ListViewModel)_controller.List(null).Model).PoliticalPartiesPerPage;

            // Assert
            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public void List_GivenThreePoliticalPartiesAndTwoPoliticalPartiesPerPage_ReturnsOnePoliticalPartyOnTheSecondPage()
        {
            // Arrange
            _controller.ItemsPerPage = 2;

            // Act
            var result = ((ListViewModel)_controller.List(null, 2).Model).PoliticalPartiesPerPage;

            // Assert
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void List_GetFilteredPoliticalParties_ReturnsOnePoliticalParty()
        {
            // Arrange
            var searchText = "PoliticalParty1";
            _mock.Setup(o => o.FilterPoliticalParties(_politicalParties, searchText)).Returns(
                new List<PoliticalParty> {
                    new PoliticalParty { Id = 1, Name = "PoliticalParty1" }});

            // Act
            var result = ((ListViewModel)_controller.List(searchText).Model).PoliticalPartiesPerPage;

            // Assert
            Assert.AreEqual(1, result.Count());
        }
        #endregion

        #region Test Edit (Get) Method
        [Test]
        public void Edit_GetPoliticalParty_ReturnsCreateViewModel()
        {
            // Arrange
            var politicalPartyId = 1;
            _mock.Setup(s => s.FindPoliticalParty(politicalPartyId)).Returns(new PoliticalParty
            {
                Id = politicalPartyId,
                Name = "PoliticalParty1",
                Abbreviation = "PP1"
            });

            // Act
            var result = (CreateViewModel)_controller.Edit(politicalPartyId).Model;

            // Assert
            Assert.AreEqual("PoliticalParty1", result.Name);
        }

        [Test]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void Edit_GetNonexistentPoliticalParty_ThrowsException()
        {
            // Arrange
            var politicalPartyId = 101;
            PoliticalParty politicalParty = null;
            _mock.Setup(s => s.FindPoliticalParty(politicalPartyId)).Returns(politicalParty);

            // Act
            var result = (CreateViewModel)_controller.Edit(politicalPartyId).Model;

            // Assert
            Assert.IsNull(result);
        }
        #endregion

        #region Test Edit (Post) Method
        [Test]
        public void Edit_PostNewPoliticalParty_ReturnsRedirectResult()
        {
            // Arrange
            var model = new CreateViewModel
            {
                Name = "NewPoliticalParty"
            };

            // Act
            var result = _controller.Edit(model);

            // Assert
            _mock.Verify(m => m.SavePoliticalParty(It.IsAny<PoliticalParty>()), Times.Once());
            Assert.IsNotNull(_controller.TempData["message-success"]);
            Assert.AreEqual("Political party NewPoliticalParty has been successfully saved.",
                _controller.TempData["message-success"]);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
        }

        [Test]
        public void Edit_PostExistingPoliticalPartyWithValidChanges_ReturnsRedirectResult()
        {
            // Arrange
            var model = new CreateViewModel
            {
                Id = 2,
                Name = "PoliticalParty 2"
            };
            var politicalParty = new PoliticalParty
            {
                Id = 2,
                Name = "PoliticalParty2"
            };
            _mock.Setup(d => d.FindPoliticalParty(model.Id)).Returns(politicalParty);

            // Act
            var result = _controller.Edit(model);

            // Assert
            _mock.Verify(m => m.SavePoliticalParty(It.IsAny<PoliticalParty>()));
            Assert.IsNotNull(_controller.TempData["message-success"]);
            Assert.AreEqual("Political party PoliticalParty 2 has been successfully saved.",
                _controller.TempData["message-success"]);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
        }
        #endregion

        #region Test Delete Method
        [Test]
        public void Delete_GetValidPoliticalParty_ReturnsSuccessMessage()
        {
            // Arrange
            var politicalPartyId = 1;
            var politicalParty = new PoliticalParty
            {
                Id = politicalPartyId,
                Name = "PoliticalParty1"
            };
            _mock.Setup(s => s.FindPoliticalParty(politicalPartyId)).Returns(politicalParty);

            // Act
            _controller.Delete(politicalPartyId);

            // Assert
            _mock.Verify(m => m.DeletePoliticalParty(politicalParty), Times.Once());
            Assert.IsNotNull(_controller.TempData["message-success"]);
            Assert.AreEqual("Political party " + politicalParty.Name + " has been successfully deleted.",
                _controller.TempData["message-success"]);
        }

        [Test]
        public void Delete_PostMultiplePoliticalParties_ReturnsSuccessMessage()
        {
            // Arrange
            var politicalParties = new int[] { 1, 2, 3 };
            _mock.Setup(s => s.FindPoliticalParty(politicalParties[0])).Returns(new PoliticalParty { Id = politicalParties[0] });
            _mock.Setup(s => s.FindPoliticalParty(politicalParties[1])).Returns(new PoliticalParty { Id = politicalParties[1] });
            _mock.Setup(s => s.FindPoliticalParty(politicalParties[2])).Returns(new PoliticalParty { Id = politicalParties[2] });

            // Act
            _controller.Delete(politicalParties);

            // Assert
            _mock.Verify(m => m.DeletePoliticalParty(It.IsAny<PoliticalParty>()), Times.Exactly(politicalParties.Count()));
            Assert.IsNotNull(_controller.TempData["message-success"]);
            Assert.AreEqual(politicalParties.Count() + " political party(s) has been successfully deleted.",
                _controller.TempData["message-success"]);
        }

        [Test]
        public void Delete_PostNoPoliticalParty_ReturnsInfoMessage()
        {
            // Arrange
            int[] politicalParties = null;

            // Act
            _controller.Delete(politicalParties);

            // Assert
            _mock.Verify(m => m.DeletePoliticalParty(It.IsAny<PoliticalParty>()), Times.Never);
            Assert.IsNotNull(_controller.TempData["message-info"]);
            Assert.AreEqual("None of the political parties has been selected for delete action.",
                _controller.TempData["message-info"]);
        }
        #endregion
    }
}
