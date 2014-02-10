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

        #region Test Edit (Get) Method
        [Test]
        public void Edit_GetDistrictType_ReturnsCreateViewModel()
        {
            // Arrange
            var districtTypeId = 1;
            _mock.Setup(s => s.FindDistrictType(districtTypeId)).Returns(new DistrictType
            {
                Id = districtTypeId,
                Name = "DistrictType1",
                ParentDistrictType = new DistrictType { Id = 1 }
            });

            // Act
            var result = (CreateViewModel)_controller.Edit(districtTypeId).Model;

            // Assert
            Assert.AreEqual("DistrictType1", result.Name);
        }

        [Test]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void Edit_GetNonexistentDistrictType_ThrowsException()
        {
            // Arrange
            var districtTypeId = 101;
            DistrictType districtType = null;
            _mock.Setup(s => s.FindDistrictType(districtTypeId)).Returns(districtType);

            // Act
            var result = (CreateViewModel)_controller.Edit(101).Model;

            // Assert
            Assert.IsNull(result);
        }
        #endregion

        #region Test Edit (Post) Method
        [Test]
        public void Edit_PostNewDistrictType_ReturnsRedirectResult()
        {
            // Arrange
            var model = new CreateViewModel
            {
                Name = "NewDistrictType",
                Abbreviation = "NDT",
            };
            //var districtType = model.ConvertModelToDistrictType(model);

            // Act
            var result = _controller.Edit(model);

            // Assert
            _mock.Verify(m => m.SaveDistrictType(It.IsAny<DistrictType>()), Times.Once());
            Assert.IsNotNull(_controller.TempData["message-success"]);
            Assert.AreEqual("District type NewDistrictType has been successfully saved.",
                _controller.TempData["message-success"]);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
        }

        [Test]
        public void Edit_PostExistingDistrictTypeWithValidChanges_ReturnsRedirectResult()
        {
            // Arrange
            var model = new CreateViewModel
            {
                Id = 2,
                Name = "District Type 2",
                Abbreviation = "DT2"
            };
            //var project = model.ConvertModelToProject(model);

            // Act
            var result = _controller.Edit(model);

            // Assert
            _mock.Verify(m => m.SaveDistrictType(It.IsAny<DistrictType>()));
            Assert.IsNotNull(_controller.TempData["message-success"]);
            Assert.AreEqual("District type District Type 2 has been successfully saved.",
                _controller.TempData["message-success"]);
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
        }
        #endregion

        #region Test Delete Method
        [Test]
        public void Delete_PostValidDistrictType_ReturnsSuccessMessage()
        {
            // Arrange
            var districtType = new DistrictType { Id = 1, Name = "DistrictType1" };

            // Act
            _controller.Delete(districtType.Id, districtType.Name);

            // Assert
            _mock.Verify(m => m.DeleteDistrictType(It.IsAny<DistrictType>()), Times.Once());
            Assert.IsNotNull(_controller.TempData["message-success"]);
            Assert.AreEqual("District type " + districtType.Name + " has been successfully deleted.",
                _controller.TempData["message-success"]);
        }
        #endregion
    }
}
