using EED.DAL;
using EED.Domain;
using EED.Service.District;
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

            _service = new DistrictService(_mock.Object);
        }

        [Test]
        public void FindAllDistricts_GivenThreeDistricts_ReturnsThreeDistricts()
        {
            // Act
            var result = _service.FindAllDistricts();

            // Assert
            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public void FindAllDistrictsFromProject_GivenTwoDistrictsFromSpecifiedProject_ReturnsTwoDistricts()
        {
            // Arrange
            var projectId = 2;

            // Act
            var result = _service.FindAllDistrictsFromProject(projectId);

            // Assert
            Assert.AreEqual(2, result.Count());
        }
    }
}
