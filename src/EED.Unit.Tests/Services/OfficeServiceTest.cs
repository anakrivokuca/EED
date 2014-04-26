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

        #region Test FilterOffices Method
        [Test]
        public void FilterOffices_ByName_ReturnsOneOffice()
        {
            // Arrange
            var offices = _mock.Object.FindAll();

            // Act
            var resultByName = _service.FilterOffices(offices, "Office1");

            // Assert
            var officesList = resultByName.ToList();
            Assert.AreEqual(1, officesList.Count);
            Assert.AreEqual("Office1", officesList[0].Name);
        }

        [Test]
        public void FilterOffices_ByIncorrectValues_ReturnsOfficesWithoutError()
        {
            // Arrange
            var offices = _mock.Object.FindAll();

            // Act
            var resultWithSpaces = _service.FilterOffices(offices, "  Office1   ");
            var resultWithNonexistentOffice = _service.FilterOffices(offices, "NonexistentOffice");

            // Assert
            var officesList = resultWithSpaces.ToList();
            Assert.AreEqual(1, officesList.Count,
                "One office should be listed with specified criteria.");
            Assert.AreEqual("Office1", officesList[0].Name,
                "Office with specified criteria should be Office1.");

            officesList = resultWithNonexistentOffice.ToList();
            Assert.AreEqual(0, officesList.Count,
                "No office should be listed with specified criteria.");
        }
        #endregion

        #region Test SaveOffice Method
        [Test]
        public void SaveOffice_NewValidOffice_DoesNotThrowError()
        {
            // Arrange
            var office = new Office
            {
                Name = "NewOffice",
                DistrictType = new DistrictType { Id = 1 },
            };

            // Act
            _service.SaveOffice(office);

            // Assert
            _mock.Verify(d => d.Save(office), Times.Once());
        }
        #endregion
    }
}
