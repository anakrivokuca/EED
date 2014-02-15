using EED.DAL;
using EED.Domain;
using EED.Service.District;
using EED.Service.Project;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace EED.Unit.Tests.Services
{
    [TestFixture]
    class DistrictServiceTest
    {
        private Mock<IRepository<District>> _mock;
        private IDistrictService _service;

        [SetUp]
        public void SetUp_DistrictServiceTest()
        {
            // Arrange
            _mock = new Mock<IRepository<District>>();
            _mock.Setup(d => d.FindAll()).Returns(new List<District> { 
                new District { Id = 1, Name = "District1", 
                    DistrictType = new DistrictType { Id = 1, Name = "DistrictType1"}, 
                    Project = new ElectionProject { Id = 1 }},
                new District { Id = 2, Name = "District2", 
                    DistrictType = new DistrictType { Id = 2, Name = "DistrictType2"}, 
                    Project = new ElectionProject { Id = 2 }},
                new District { Id = 3, Name = "District3", 
                    DistrictType = new DistrictType { Id = 2, Name = "DistrictType2"}, 
                    Project = new ElectionProject { Id = 2 }}});

            //var mockProjectService = new Mock<IProjectService>();

            _service = new DistrictService(_mock.Object
                //, mockProjectService.Object
                );
        }

        #region Test FindAllDistricts Method
        [Test]
        public void FindAllDistricts_GivenThreeDistricts_ReturnsThreeDistricts()
        {
            // Act
            var result = _service.FindAllDistricts();

            // Assert
            Assert.AreEqual(3, result.Count());
        }
        #endregion

        #region Test FilterDistricts Method
        [Test]
        public void FilterDistricts_ByNameAndDistrictType_ReturnsOneDistrict()
        {
            // Arrange
            var districts = _mock.Object.FindAll();
            var districtName = "District1";

            // Act
            var resultByName = _service.FilterDistricts(districts, districtName, 0);
            var resultByDistrictType = _service.FilterDistricts(districts, "", 2);

            // Assert
            var districtList = resultByName.ToList();
            Assert.AreEqual(1, districtList.Count());
            Assert.AreEqual(districtName, districtList[0].Name,
                "District with specified name should be " + districtName + ".");

            districtList = resultByDistrictType.ToList();
            Assert.AreEqual(2, districtList.Count());
            Assert.AreEqual("District2", districtList[0].Name,
                "First district with specified district type should be District2.");
            Assert.AreEqual("District3", districtList[1].Name,
                "Second district with specified district type should be District3.");
        }

        [Test]
        public void FilterDistricts_ByIncorrectValues_ReturnsDistrictsWithoutError()
        {
            // Arrange
            var districts = _mock.Object.FindAll();

            // Act
            var resultWithSpaces = _service.FilterDistricts(districts, "  District1   ", 0);
            var resultWithNonexistentDistrict = _service.FilterDistricts(districts, "NonexistentDistrict", 5);

            // Assert
            var districtTypesList = resultWithSpaces.ToList();
            Assert.AreEqual(1, districtTypesList.Count(),
                "One district types should be listed with specified criteria.");
            Assert.AreEqual("District1", districtTypesList[0].Name,
                "District type with specified criteria should be District1.");

            Assert.AreEqual(0, resultWithNonexistentDistrict.Count(),
                "No district type should be listed with specified criteria.");
        }
        #endregion

        #region Test SaveDistrict Method
        [Test]
        public void SaveDistrict_NewValidDistrict_DoesNotThrowError()
        {
            // Arrange
            var district = new District
            {
                Name = "NewDistrict",
                DistrictType = new DistrictType { Id = 1 },
            };

            // Act
            _service.SaveDistrict(district);

            // Assert
            _mock.Verify(d => d.Save(district), Times.Once());
        }
        #endregion

        #region Test DeleteDistrict Method
        [Test]
        public void DeleteDistrict_ValidDistrict_DoesNotThrowError()
        {
            // Arrange
            var district = new District { Id = 1, Name = "District1" };

            // Act
            _service.DeleteDistrict(district);

            // Assert
            _mock.Verify(m => m.Delete(It.IsAny<District>()), Times.Once());
        }
        #endregion
    }
}
