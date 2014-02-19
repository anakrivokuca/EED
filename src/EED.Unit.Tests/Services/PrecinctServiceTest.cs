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

        #region Test FindAllPrecinctsFromProject Method
        [Test]
        public void FindAllPrecinctsFromProject_GivenTwoPrecinctsForSpecifiedProject_ReturnsTwoPrecincts()
        {
            // Arrange
            int projectId = 2;

            // Act
            var result = _service.FindAllPrecinctsFromProject(projectId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void FindAllPrecinctsFromProject_GivenZeroPrecinctsForSpecifiedProject_ReturnsZeroPrecincts()
        {
            // Arrange
            int projectId = 101;

            // Act
            var result = _service.FindAllPrecinctsFromProject(projectId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }
        #endregion
    }
}
