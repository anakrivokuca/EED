using EED.Domain;
using EED.Service.Controller.Offices;
using EED.Ui.Web.Controllers;
using EED.Ui.Web.Models.Offices;
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
    class OfficeControllerTest
    {
        private Mock<IOfficeServiceController> _mock;
        private OfficeController _controller;
        private IEnumerable<Office> _offices;

        [SetUp]
        public void SetUp_OfficeControllerTest()
        {
            // Arrange
            _offices = new List<Office> { 
                new Office { Id = 1, Name = "Office1", 
                    DistrictType = new DistrictType { Id = 1, Name = "DistrictType1"}, 
                    Project = new ElectionProject { Id = 1 }},
                new Office { Id = 2, Name = "Office2", 
                    DistrictType = new DistrictType { Id = 2, Name = "DistrictType2"}, 
                    Project = new ElectionProject { Id = 1 }},
                new Office { Id = 3, Name = "Office3", 
                    DistrictType = new DistrictType { Id = 2, Name = "DistrictType2"}, 
                    Project = new ElectionProject { Id = 1 }}};

            _mock = new Mock<IOfficeServiceController>();

            _mock.Setup(p => p.FindProject(1)).Returns(new ElectionProject
            {
                Id = 1,
                DistrictTypes = new List<DistrictType> { new DistrictType { Id = 1 }, 
                    new DistrictType { Id = 2, ParentDistrictType = new DistrictType { Id = 1 } } },
                Offices = _offices.ToList()
            });

            _controller = new OfficeController(_mock.Object);

            var session = new Mock<HttpSessionStateBase>();
            var context = new Mock<HttpContextBase>();
            session.SetupGet(s => s["projectId"]).Returns("1");
            context.SetupGet(x => x.Session).Returns(session.Object);
            _controller.ControllerContext = new ControllerContext(context.Object,
                new RouteData(), _controller);
        }

        #region Test List Method
        [Test]
        public void List_GivenThreeOffices_ReturnsThreeOffices()
        {
            // Act
            var result = ((ListViewModel)_controller.List(null).Model).OfficesPerPage;

            // Assert
            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public void List_GivenThreeOfficesAndTwoOfficesPerPage_ReturnsOneOfficeOnTheSecondPage()
        {
            // Arrange
            _controller.ItemsPerPage = 2;

            // Act
            var result = ((ListViewModel)_controller.List(null, 2).Model).OfficesPerPage;

            // Assert
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void List_GetFilteredOffices_ReturnsOneOffice()
        {
            // Arrange
            var searchText = "Office1";
            _mock.Setup(o => o.FilterOffices(_offices, searchText)).Returns(
                new List<Office> {
                    new Office { Id = 1, Name = "Office1" }});

            // Act
            var result = ((ListViewModel)_controller.List(searchText).Model).OfficesPerPage;

            // Assert
            Assert.AreEqual(1, result.Count());
        }
        #endregion
    }
}
