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
            
            _mock.Setup(p => p.FindProject(1)).Returns(new ElectionProject
            {
                Id = 1,
                Districts = new List<District> { 
                    new District { Id = 1, Name = "District1", 
                        DistrictType = new DistrictType { Id = 1, Name = "DistrictType1"}, 
                        Project = new ElectionProject { Id = 1 }},
                    new District { Id = 2, Name = "District2", 
                        DistrictType = new DistrictType { Id = 2, Name = "DistrictType2"}, 
                        Project = new ElectionProject { Id = 1 }},
                    new District { Id = 3, Name = "District3", 
                        DistrictType = new DistrictType { Id = 2, Name = "DistrictType2"}, 
                        Project = new ElectionProject { Id = 1 }}},
                Precincts = _precincts.ToList()
            });
            
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

        #region Test Edit (Get) Method
        [Test]
        public void Edit_GetPrecinct_ReturnsCreateViewModel()
        {
            // Arrange
            var precinctId = 1;
            _mock.Setup(s => s.FindPrecinct(precinctId)).Returns(new Precinct
            {
                Id = precinctId,
                Name = "Precinct1",
                Districts = new List<District> { new District { Id = 1, Name = "District1" } }
            });

            // Act
            var result = (CreateViewModel)_controller.Edit(precinctId).Model;

            // Assert
            Assert.AreEqual("Precinct1", result.Name);
        }

        [Test]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void Edit_GetNonexistentPrecinct_ThrowsException()
        {
            // Arrange
            var precinctId = 101;
            Precinct precinct = null;
            _mock.Setup(s => s.FindPrecinct(precinctId)).Returns(precinct);

            // Act
            var result = (CreateViewModel)_controller.Edit(101).Model;

            // Assert
            Assert.IsNull(result);
        }
        #endregion

        #region Test Edit (Post) Method
        [Test]
        public void Edit_PostNewPrecinct_ReturnsRedirectResult()
        {
            // Arrange
            var model = new CreateViewModel { Name = "NewPrecinct" };

            // Act
            var result = _controller.Edit(model);

            // Assert
            _mock.Verify(m => m.SavePrecinct(It.IsAny<Precinct>()), Times.Once());
            Assert.IsNotNull(_controller.TempData["message-success"]);
            Assert.AreEqual("Precinct NewPrecinct has been successfully saved.",
                _controller.TempData["message-success"]);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
        }

        [Test]
        public void Edit_PostExistingPrecinctWithValidChanges_ReturnsRedirectResult()
        {
            // Arrange
            var model = new CreateViewModel
            {
                Id = 2,
                Name = "Precinct 2"
            };

            // Act
            var result = _controller.Edit(model);

            // Assert
            _mock.Verify(m => m.SavePrecinct(It.IsAny<Precinct>()));
            Assert.IsNotNull(_controller.TempData["message-success"]);
            Assert.AreEqual("Precinct Precinct 2 has been successfully saved.",
                _controller.TempData["message-success"]);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
        }
        #endregion
    }
}
