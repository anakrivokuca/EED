using EED.DAL;
using EED.Domain;
using EED.Service.Precincts;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace EED.Unit.Tests.Services
{
    [TestFixture]
    class PrecinctServiceTest
    {
        private Mock<IRepository<Precinct>> _mock;
        private IPrecinctService _service;

        [SetUp]
        public void SetUp_PrecinctServiceTest()
        {
            // Arrange
            _mock = new Mock<IRepository<Precinct>>();
            _mock.Setup(r => r.FindAll()).Returns(new List<Precinct> {
                new Precinct { Id = 1, Name = "Precinct1", 
                    Project = new ElectionProject { Id = 1} },
                new Precinct { Id = 2, Name = "Precinct2", 
                    Project = new ElectionProject { Id = 2} },
                new Precinct { Id = 3, Name = "Precinct3", 
                    Project = new ElectionProject { Id = 2} },});

            _service = new PrecinctService(_mock.Object);
        }

        #region Test FindAllPrecincts Method
        [Test]
        public void FindAllPrecincts_GivenThreePrecincts_ResturnsThreePrecincts()
        {
            // Act
            var result = _service.FindAllPrecincts();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
        }
        #endregion

        #region Test FindPrecinct Method
        [Test]
        public void FindPrecinct_GoodPrecinct_ReturnsPrecinct()
        {
            // Arrange
            var precinctId = 1;
            _mock.Setup(r => r.Find(precinctId)).Returns(new Precinct { Id = 1, Name = "Precinct1" });

            // Act
            var result = _service.FindPrecinct(precinctId);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void FindPrecinct_NonExistentPrecinct_ReturnsNull()
        {
            // Arrange
            var precinctId = 101;
            Precinct precinct = null;
            _mock.Setup(r => r.Find(precinctId)).Returns(precinct);

            //Act
            var result = _service.FindPrecinct(precinctId);

            //Assert
            Assert.IsNull(result);
        }
        #endregion

        #region Test SavePrecinct Method
        [Test]
        public void SavePrecinct_NewValidPrecinct_DoesNotThrowError()
        {
            // Arrange
            var precinct = new Precinct { Name = "NewPrecinct" }; 

            // Act
            _service.SavePrecinct(precinct);

            // Assert
            _mock.Verify(m => m.Save(precinct));
        }
        #endregion
    }
}
