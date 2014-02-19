using EED.Domain;
using EED.Service.Controller.Precincts;
using EED.Ui.Web.Controllers;
using EED.Ui.Web.Models.Precincts;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EED.Unit.Tests.Controllers
{
    [TestFixture]
    class PrecinctControllerTest
    {
        private Mock<IPrecinctServiceController> _mock;
        private PrecinctController _controller;
        private IEnumerable<Precinct> _precincts;

        [SetUp]
        public void SetUp_PrecinctControllerTest()
        {
            // Arrange
            _precincts = new List<Precinct> {
                new Precinct { Id = 1, Name = "Precinct1", 
                    Project = new ElectionProject { Id = 1} },
                new Precinct { Id = 2, Name = "Precinct2", 
                    Project = new ElectionProject { Id = 1} },
                new Precinct { Id = 3, Name = "Precinct3", 
                    Project = new ElectionProject { Id = 1} },};

            _mock = new Mock<IPrecinctServiceController>();
            _mock.Setup(s => s.FindAllPrecinctsFromProject(1)).Returns(_precincts);

            _controller = new PrecinctController(_mock.Object);

            var session = new Mock<HttpSessionStateBase>();
            var context = new Mock<HttpContextBase>();
            session.SetupGet(s => s["projectId"]).Returns("1");
            context.SetupGet(x => x.Session).Returns(session.Object);
            _controller.ControllerContext = new ControllerContext(context.Object,
                new RouteData(), _controller);
        }

        #region Test List Method
        [Test]
        public void List_GivenThreePrecincts_ReturnsThreePrecincts()
        {
            // Act
            var result = ((ListViewModel)_controller.List().Model).Precincts;

            // Assert
            Assert.AreEqual(3, result.Count());
        }
        #endregion
    }
}
