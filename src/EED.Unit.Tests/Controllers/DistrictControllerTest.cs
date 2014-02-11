using EED.Domain;
using EED.Service.District;
using EED.Service.District_Type;
using EED.Ui.Web.Controllers;
using EED.Ui.Web.Models.District;
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
        private Mock<IDistrictService> _mock;
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

            _mock = new Mock<IDistrictService>();
            _mock.Setup(d => d.FindAllDistrictsFromProject(1)).Returns(_districts);

            var mockDistrictTypeService = new Mock<IDistrictTypeService>();
            mockDistrictTypeService.Setup(dt => dt.FindAllDistrictTypesFromProject(1))
                .Returns(new List<DistrictType> {
                    new DistrictType { Id = 1, Name = "DistrictType1", 
                        Project = new ElectionProject { Id = 1} },
                    new DistrictType { Id = 2, Name = "DistrictType2", 
                        Project = new ElectionProject { Id = 1} }});

            _controller = new DistrictController(_mock.Object, mockDistrictTypeService.Object);

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
            var districtTypeId = 1;
            _mock.Setup(d => d.FilterDistricts(_districts, searchText, districtTypeId)).Returns(
                new List<District> { new District { Id = districtTypeId, Name = searchText }});

            // Act
            var result = ((ListViewModel)_controller.List(searchText, 1).Model).DistrictsPerPage;

            // Assert
            Assert.AreEqual(1, result.Count());
        }
        #endregion
    }
}
