using EED.Domain;
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
    }
}
