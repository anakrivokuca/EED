using EED.DAL;
using EED.Domain;
using EED.Service.District_Type;
using EED.Service.Project;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EED.Unit.Tests.Services
{
    [TestFixture]
    class DistrictTypeServiceTest
    {
        private Mock<IRepository<DistrictType>> _mock;
        private IDistrictTypeService _service;

        [SetUp]
        public void SetUp_DistrictTypeServiceTest()
        {
            _mock = new Mock<IRepository<DistrictType>>();
            _mock.Setup(r => r.FindAll()).Returns(new List<DistrictType> {
                new DistrictType { Id = 1, Name = "DistrictType1", 
                    Project = new ElectionProject { Id = 1} },
                new DistrictType { Id = 2, Name = "DistrictType2", 
                    Project = new ElectionProject { Id = 2} },
                new DistrictType { Id = 3, Name = "DistrictType3", 
                    ParentDistrictType = new DistrictType { Id = 2 },
                    Project = new ElectionProject { Id = 2} },});

            _service = new DistrictTypeService(_mock.Object);
        }

        #region Test FindAllDistrictTypes Method
        [Test]
        public void FindAllDistrictTypes_GivenThreeDistrictTypes_ResturnsThreeDistrictTypes()
        {
            // Act
            var result = _service.FindAllDistrictTypes();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
        }
        #endregion

        #region Test FindAllDistrictTypesFromProject Method
        [Test]
        public void FindAllDistrictTypesFromProject_GivenTwoDistrictTypesForSpecifiedProject_ReturnsTwoDistrictTypes()
        {
            // Arrange
            int projectId = 2;

            // Act
            var result = _service.FindAllDistrictTypesFromProject(projectId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void FindAllDistrictTypesFromProject_GivenZeroDistrictTypesForSpecifiedProject_ReturnsZeroDistrictTypes()
        {
            // Arrange
            int projectId = 101;

            // Act
            var result = _service.FindAllDistrictTypesFromProject(projectId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }
        #endregion

        #region Test FindDistrictType Method
        [Test]
        public void FindDistrictType_GoodDistrictType_ReturnsDistrictType()
        {
            // Arrange
            var districtTypeId = 1;
            _mock.Setup(r => r.Find(districtTypeId)).Returns(new DistrictType { Id = 1, Name = "DistrictType1" });

            // Act
            var result = _service.FindDistrictType(districtTypeId);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void FindDistrictType_NonExistentDistrictType_ReturnsNull()
        {
            // Arrange
            var districtTypeId = 101;
            DistrictType districtType = null;
            _mock.Setup(r => r.Find(districtTypeId)).Returns(districtType);

            //Act
            var result = _service.FindDistrictType(districtTypeId);

            //Assert
            Assert.IsNull(result);
        }
        #endregion

        #region Test FilterDistrictTypes Method
        [Test]
        public void FilterDistrictTypes_ByName_ReturnsOneDistrictType()
        {
            // Arrange
            var districtTypes = _mock.Object.FindAll();

            // Act
            var resultByName = _service.FilterDistrictTypes(districtTypes, "DistrictType1");

            // Assert
            var districtTypesList = resultByName.ToList();
            Assert.AreEqual(1, districtTypesList.Count);
            Assert.AreEqual("DistrictType1", districtTypesList[0].Name,
                "Project with specified name should be DistrictType1.");
        }

        [Test]
        public void FilterDistrictTypes_ByIncorrectValues_ReturnsDistrictTypesWithoutError()
        {
            // Arrange
            var districtTypes = _mock.Object.FindAll();

            // Act
            var resultWithSpaces = _service.FilterDistrictTypes(districtTypes, "  DistrictType1   ");
            var resultWithNonexistentDistrictType = _service.FilterDistrictTypes(districtTypes, "NonexistentDistrictType");

            // Assert
            var districtTypesList = resultWithSpaces.ToList();
            Assert.AreEqual(1, districtTypesList.Count,
                "One district types should be listed with specified criteria.");
            Assert.AreEqual("DistrictType1", districtTypesList[0].Name,
                "District type with specified criteria should be DistrictType1.");

            districtTypesList = resultWithNonexistentDistrictType.ToList();
            Assert.AreEqual(0, districtTypesList.Count,
                "No district type should be listed with specified criteria.");
        }
        #endregion

        #region Test SaveDistrictType Method
        [Test]
        public void SaveDistrictType_NewValidDistrictType_DoesNotThrowError()
        {
            // Arrange
            var districtType = new DistrictType
            {
                Name = "NewPDistrictType",
                Abbreviation = "NDT",
                ParentDistrictType = new DistrictType { Id = 1 }
            };

            // Act
            _service.SaveDistrictType(districtType);

            // Assert
            _mock.Verify(m => m.Save(districtType));
        }
        #endregion

        #region Test DeleteDistrictType Method
        [Test]
        public void DeleteDistrictType_ValidDistrictType_DoesNotThrowError()
        {
            // Arrange
            var districtType = new DistrictType { Id = 1, Name = "DistrictType1" };

            // Act
            _service.DeleteDistrictType(districtType);

            // Assert
            _mock.Verify(m => m.Delete(It.IsAny<DistrictType>()), Times.Once());
        }
        #endregion
    }
}
