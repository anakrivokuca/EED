using EED.Domain;
using EED.Service.District;
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

        [SetUp]
        public void SetUp_DistrictControllerTest()
        {
            // Arrange
            _mock = new Mock<IDistrictService>();
            _mock.Setup(d => d.FindAllDistrictsFromProject(1)).Returns(new List<District> { 
                new District { Id = 1, Name = "District1", 
                    DistrictType = new DistrictType { Id = 1, Name = "DistrictType1"}, 
                    Project = new ElectionProject { Id = 1 }},
                new District { Id = 2, Name = "District2", 
                    DistrictType = new DistrictType { Id = 2, Name = "DistrictType2"}, 
                    Project = new ElectionProject { Id = 1 }},
                new District { Id = 3, Name = "District3", 
                    DistrictType = new DistrictType { Id = 2, Name = "DistrictType2"}, 
                    Project = new ElectionProject { Id = 1 }}});

            _controller = new DistrictController(_mock.Object);

            var session = new Mock<HttpSessionStateBase>();
            var context = new Mock<HttpContextBase>();
            session.SetupGet(s => s["projectId"]).Returns("1");
            context.SetupGet(x => x.Session).Returns(session.Object);
            _controller.ControllerContext = new ControllerContext(context.Object,
                new RouteData(), _controller);
        }

        [Test]
        public void List_GivenThreeDistricts_ReturnsThreeDistricts()
        {
            // Act
            var result = ((ListViewModel)_controller.List().Model).Districts;

            // Assert
            Assert.AreEqual(3, result.Count());
        }
    }
}
