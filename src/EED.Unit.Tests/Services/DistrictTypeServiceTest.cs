﻿using EED.DAL;
using EED.Domain;
using EED.Service.District_Type;
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
    }
}
