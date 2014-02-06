using EED.Domain;
using EED.Service.District_Type;
using EED.Ui.Web.Controllers;
using EED.Ui.Web.Models.District_Type;
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
    class DistrictTypeControllerTest
    {
        private Mock<IDistrictTypeService> _mock;
        private DistrictTypeController _controller;
        private IEnumerable<DistrictType> _districtTypes;

        [SetUp]
        public void SetUp_ProjectControllerTest()
        {
            // Arrange
            _districtTypes = new List<DistrictType> {
                new DistrictType { Id = 1, Name = "DistrictType1", 
                    Project = new ElectionProject { Id = 1} },
                new DistrictType { Id = 2, Name = "DistrictType2", 
                    Project = new ElectionProject { Id = 1} },
                new DistrictType { Id = 3, Name = "DistrictType3", 
                    ParentDistrictType = new DistrictType { Id = 2 },
                    Project = new ElectionProject { Id = 1} },};

            _mock = new Mock<IDistrictTypeService>();
            _mock.Setup(s => s.FindAllDistrictTypesFromProject(1)).Returns(_districtTypes);

            _controller = new DistrictTypeController(_mock.Object);

            var session = new Mock<HttpSessionStateBase>();
            var context = new Mock<HttpContextBase>();
            session.SetupGet(s => s["projectId"]).Returns("1");
            context.SetupGet(x => x.Session).Returns(session.Object);
            _controller.ControllerContext = new ControllerContext(context.Object,
                new RouteData(), _controller);
        }

        #region Test List Method
        [Test]
        public void List_GivenTwoDistrictTypes_ReturnsTwoDistrictTypes()
        {
            // Act
            var result = ((ListViewModel)_controller.List(null).Model).DistrictTypes;

            // Assert
            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public void List_GetFilteredDistrictTypes_ReturnsOneDistrictType()
        {
            // Arrange
            var searchText = "DistrictType1";
            _mock.Setup(dt => dt.FilterDistrictTypes(_districtTypes, searchText)).Returns(
                new List<DistrictType> {
                    new DistrictType { Id = 1, Name = "DistrictType1" }});

            // Act
            var result = ((ListViewModel)_controller.List(searchText).Model).DistrictTypes;

            // Assert
            Assert.AreEqual(1, result.Count());
        }
        #endregion
    }
}
