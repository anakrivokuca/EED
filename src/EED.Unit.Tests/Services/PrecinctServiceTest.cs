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
                    Districts = new List<District> { new District { Id = 2 }},
                    Project = new ElectionProject { Id = 2} },
                new Precinct { Id = 3, Name = "Precinct3", 
                    Districts = new List<District> { new District { Id = 2 }},
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

        #region Test FilterPrecincts Method
        [Test]
        public void FilterPrecincts_ByNameAndDistrict_ReturnsOnePrecinct()
        {
            // Arrange
            var precincts = _mock.Object.FindAll();
            var precinctName = "Precinct1";

            // Act
            var resultByName = _service.FilterPrecincts(precincts, precinctName, 0);
            var resultByDistrict = _service.FilterPrecincts(precincts, "", 2);

            // Assert
            var precinctList = resultByName.ToList();
            Assert.AreEqual(1, precinctList.Count());
            Assert.AreEqual(precinctName, precinctList[0].Name,
                "Precinct with specified name should be " + precinctName + ".");

            precinctList = resultByDistrict.ToList();
            Assert.AreEqual(2, precinctList.Count());
            Assert.AreEqual("Precinct2", precinctList[0].Name,
                "First precinct with specified district should be Precinct2.");
            Assert.AreEqual("Precinct3", precinctList[1].Name,
                "Second precinct with specified district type should be Precinct3.");
        }

        [Test]
        public void FilterPrecincts_ByIncorrectValues_ReturnsPrecinctsWithoutError()
        {
            // Arrange
            var precincts = _mock.Object.FindAll();

            // Act
            var resultWithSpaces = _service.FilterPrecincts(precincts, "  Precinct1   ", 0);
            var resultWithNonexistentPrecinct = _service.FilterPrecincts(precincts, "NonexistentPrecinct", 5);

            // Assert
            var precinctsList = resultWithSpaces.ToList();
            Assert.AreEqual(1, precinctsList.Count(),
                "One precinct should be listed with specified criteria.");
            Assert.AreEqual("Precinct1", precinctsList[0].Name,
                "Precinct with specified criteria should be Precinct1.");

            Assert.AreEqual(0, resultWithNonexistentPrecinct.Count(),
                "No precinct should be listed with specified criteria.");
        }
        #endregion

        #region Test SavePrecinct Method
        [Test]
        public void SavePrecinct_NewValidPrecinct_DoesNotThrowError()
        {
            // Arrange
            var precinct = new Precinct
            {
                Name = "NewPrecinct",
                Districts = new List<District> { new District { Id = 1 } }
            }; 

            // Act
            _service.SavePrecinct(precinct);

            // Assert
            _mock.Verify(m => m.Save(precinct));
        }

        [Test]
        public void SavePrecinct_PrecinctWithSelectedChildDistrict_SavesPrecinctWithParentDistricts()
        {
            // Arrange
            var jurisdiction = new District { Id = 1 };
            var precinct = new Precinct 
            { 
                Name = "Precinct1",
                Districts = new List<District> { 
                    new District { Id = 2, ParentDistrict = jurisdiction },
                    new District { Id = 3, ParentDistrict = jurisdiction },
                    new District { Id = 4, ParentDistrict = new District { 
                        Id = 5, ParentDistrict = jurisdiction }}}
            };

            // Act
            _service.SavePrecinct(precinct);

            // Assert
            _mock.Verify(m => m.Save(precinct));
            Assert.AreEqual(5, precinct.Districts.Count);
        }

        [Test]
        public void SavePrecinct_PrecinctWithNoDistrictsSelected_DoesNotThrowError()
        {
            // Arrange
            var precinct = new Precinct { Name = "Precinct1" };

            // Act
            _service.SavePrecinct(precinct);

            // Assert
            _mock.Verify(m => m.Save(precinct));
        }
        #endregion

        #region Test DeletePrecinct Method
        [Test]
        public void DeletePrecinct_ValidPrecinct_DoesNotThrowError()
        {
            // Arrange
            var precinct = new Precinct { Id = 1, Name = "Precinct1" };

            // Act
            _service.DeletePrecinct(precinct);

            // Assert
            _mock.Verify(m => m.Delete(It.IsAny<Precinct>()), Times.Once());
        }
        #endregion
    }
}
