using EED.Domain;
using EED.Service.Controller.District;
using EED.Ui.Web.Controllers;
using EED.Ui.Web.Models.Districts;
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
    class DistrictControllerTest
    {
        private Mock<IDistrictServiceController> _mock;
        private DistrictController _controller;
        private IEnumerable<District> _districts;

        [SetUp]
        public void SetUp_DistrictControllerTest()
        {
            // Arrange
            _districts = new List<District> { 
                new District { Id = 1, Name = "District1", 
                    DistrictType = new DistrictType { Id = 1, Name = "DistrictType1"}, 
                    Project = new ElectionProject { Id = 1 }},
                new District { Id = 2, Name = "District2", 
                    DistrictType = new DistrictType { Id = 2, Name = "DistrictType2"}, 
                    Project = new ElectionProject { Id = 1 }},
                new District { Id = 3, Name = "District3", 
                    DistrictType = new DistrictType { Id = 2, Name = "DistrictType2"}, 
                    Project = new ElectionProject { Id = 1 }}};

            _mock = new Mock<IDistrictServiceController>();

            _mock.Setup(p => p.FindProject(1)).Returns(new ElectionProject
            {
                Id = 1,
                DistrictTypes = new List<DistrictType> { new DistrictType { Id = 1 }, 
                    new DistrictType { Id = 2, ParentDistrictType = new DistrictType { Id = 1 } } },
                Districts = _districts.ToList()
            });

            _controller = new DistrictController(_mock.Object);

            var session = new Mock<HttpSessionStateBase>();
            var context = new Mock<HttpContextBase>();
            session.SetupGet(s => s["projectId"]).Returns("1");
            context.SetupGet(x => x.Session).Returns(session.Object);
            _controller.ControllerContext = new ControllerContext(context.Object,
                new RouteData(), _controller);
        }

        #region Test List Method
        [Test]
        public void List_GivenThreeDistricts_ReturnsThreeDistricts()
        {
            // Act
            var result = ((ListViewModel)_controller.List(null).Model).DistrictsPerPage;

            // Assert
            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public void List_GivenThreeDistrictsAndTwoDistrictsPerPage_ReturnsOneDistrictOnTheSecondPage()
        {
            // Arrange
            _controller.ItemsPerPage = 2;

            // Act
            var result = ((ListViewModel)_controller.List(null, 0, 2).Model).DistrictsPerPage;

            // Assert
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void List_GetFilteredDistricts_ReturnsOneDistrict()
        {
            // Arrange
            var searchText = "District1";
            var districtId = 1;
            _mock.Setup(d => d.FilterDistricts(_districts, searchText, districtId)).Returns(
                new List<District> { new District { Id = districtId, Name = searchText }});

            // Act
            var result = ((ListViewModel)_controller.List(searchText, 1).Model).DistrictsPerPage;

            // Assert
            Assert.AreEqual(1, result.Count());
        }
        #endregion

        #region Test Edit (Get) Method
        [Test]
        public void Edit_GetDistrict_ReturnsCreateViewModel()
        {
            // Arrange
            var districtId = 1;
            _mock.Setup(s => s.FindDistrict(districtId)).Returns(new District
            {
                Id = districtId,
                Name = "District1",
                DistrictType = new DistrictType { Id = 1 },
                ParentDistrict = new District { Id = 2 }
            });

            // Act
            var result = (CreateViewModel)_controller.Edit(districtId).Model;

            // Assert
            Assert.AreEqual("District1", result.Name);
        }

        [Test]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void Edit_GetNonexistentDistrict_ThrowsException()
        {
            // Arrange
            var districtId = 101;
            District district = null;
            _mock.Setup(s => s.FindDistrict(districtId)).Returns(district);

            // Act
            var result = (CreateViewModel)_controller.Edit(districtId).Model;

            // Assert
            Assert.IsNull(result);
        }
        #endregion

        #region Test Edit (Post) Method
        [Test]
        public void Edit_PostNewDistrict_ReturnsRedirectResult()
        {
            // Arrange
            var model = new CreateViewModel
            {
                Name = "NewDistrict",
                Abbreviation = "ND",
            };

            // Act
            var result = _controller.Edit(model);

            // Assert
            _mock.Verify(m => m.SaveDistrict(It.IsAny<District>()), Times.Once());
            Assert.IsNotNull(_controller.TempData["message-success"]);
            Assert.AreEqual("District NewDistrict has been successfully saved.",
                _controller.TempData["message-success"]);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
        }

        [Test]
        public void Edit_PostExistingDistrictWithValidChanges_ReturnsRedirectResult()
        {
            // Arrange
            var model = new CreateViewModel
            {
                Id = 2,
                Name = "District 2",
                Abbreviation = "D2"
            };
            var district = new District
            {
                Id = 2,
                Name = "District2",
                Abbreviation = "D2"
            };
            _mock.Setup(d => d.FindDistrict(model.Id)).Returns(district);

            // Act
            var result = _controller.Edit(model);

            // Assert
            _mock.Verify(m => m.SaveDistrict(It.IsAny<District>()));
            Assert.IsNotNull(_controller.TempData["message-success"]);
            Assert.AreEqual("District District 2 has been successfully saved.",
                _controller.TempData["message-success"]);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
        }
        #endregion

        #region Test Delete Method
        [Test]
        public void Delete_GetValidDistrict_ReturnsSuccessMessage()
        {
            // Arrange
            var districtId = 1;
            var district = new District
            {
                Id = districtId,
                Name = "District1"
            };
            _mock.Setup(s => s.FindDistrict(districtId)).Returns(district);

            // Act
            _controller.Delete(districtId);

            // Assert
            _mock.Verify(m => m.DeleteDistrict(district), Times.Once());
            Assert.IsNotNull(_controller.TempData["message-success"]);
            Assert.AreEqual("District " + district.Name + " has been successfully deleted.",
                _controller.TempData["message-success"]);
        }

        [Test]
        public void Delete_PostMultipleDistricts_ReturnsSuccessMessage()
        {
            // Arrange
            var districts = new int[] { 1, 2, 3 };
            _mock.Setup(s => s.FindDistrict(districts[0])).Returns(new District { Id = districts[0] });
            _mock.Setup(s => s.FindDistrict(districts[1])).Returns(new District { Id = districts[1] });
            _mock.Setup(s => s.FindDistrict(districts[2])).Returns(new District { Id = districts[2] });

            // Act
            _controller.Delete(districts);

            // Assert
            _mock.Verify(m => m.DeleteDistrict(It.IsAny<District>()), Times.Exactly(districts.Count()));
            Assert.IsNotNull(_controller.TempData["message-success"]);
            Assert.AreEqual(districts.Count() + " district(s) has been successfully deleted.",
                _controller.TempData["message-success"]);
        }

        [Test]
        public void Delete_PostNoDistrict_ReturnsInfoMessage()
        {
            // Arrange
            int[] districts = null;

            // Act
            _controller.Delete(districts);

            // Assert
            _mock.Verify(m => m.DeleteDistrict(It.IsAny<District>()), Times.Never);
            Assert.IsNotNull(_controller.TempData["message-info"]);
            Assert.AreEqual("None of the districts has been selected for delete action.",
                _controller.TempData["message-info"]);
        }
        #endregion
    }
}
