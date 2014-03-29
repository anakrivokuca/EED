using EED.DAL;
using EED.Domain;
using EED.Service.Offices;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace EED.Unit.Tests.Services
{
    [TestFixture]
    class OfficeServiceTest
    {
        private Mock<IRepository<Office>> _mock;
        private IOfficeService _service;

        [SetUp]
        public void SetUp_OfficeServiceTest()
        {
            // Arrange
            _mock = new Mock<IRepository<Office>>();
            _mock.Setup(d => d.FindAll()).Returns(new List<Office> { 
                new Office { Id = 1, Name = "Office1", 
                    DistrictType = new DistrictType { Id = 1, Name = "DistrictType1"}, 
                    Project = new ElectionProject { Id = 1 }},
                new Office { Id = 2, Name = "Office2", 
                    DistrictType = new DistrictType { Id = 2, Name = "DistrictType2"}, 
                    Project = new ElectionProject { Id = 2 }},
                new Office { Id = 3, Name = "Office3", 
                    DistrictType = new DistrictType { Id = 2, Name = "DistrictType2"}, 
                    Project = new ElectionProject { Id = 2 }}});

            _service = new OfficeService(_mock.Object);
        }

        #region Test FindAllOffices Method
        [Test]
        public void FindAllOffices_GivenThreeOffices_ReturnsThreeOffices()
        {
            // Act
            var result = _service.FindAllOffices();

            // Assert
            Assert.AreEqual(3, result.Count());
        }
        #endregion
    }
}
