using EED.DAL;
using EED.Domain;
using EED.Service.Political_Party;
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
    class PoliticalPartyServiceTest
    {
        private Mock<IRepository<PoliticalParty>> _mock;
        private IPoliticalPartyService _service;

        [SetUp]
        public void SetUp_PoliticalPartyServiceTest()
        {
            // Arrange
            _mock = new Mock<IRepository<PoliticalParty>>();
            _mock.Setup(d => d.FindAll()).Returns(new List<PoliticalParty> { 
                new PoliticalParty { Id = 1, Name = "PoliticalParty1", 
                    Abbreviation = "PP1", 
                    Project = new ElectionProject { Id = 1 }},
                new PoliticalParty { Id = 2, Name = "PoliticalParty2", 
                    Abbreviation = "PP2", 
                    Project = new ElectionProject { Id = 1 }},
                new PoliticalParty { Id = 3, Name = "PoliticalParty3", 
                    Abbreviation = "PP2", 
                    Project = new ElectionProject { Id = 1 }}});

            _service = new PoliticalPartyService(_mock.Object);
        }

        #region Test FindAllPoliticalParties Method
        [Test]
        public void FindAllPoliticalParties_GivenThreePoliticalParties_ReturnsThreePoliticalParties()
        {
            // Act
            var result = _service.FindAllPoliticalParties();

            // Assert
            Assert.AreEqual(3, result.Count());
        }
        #endregion

        #region Test FilterPoliticalParties Method
        [Test]
        public void FilterPoliticalParties_ByName_ReturnsOnePoliticalParty()
        {
            // Arrange
            var politicalParties = _mock.Object.FindAll();

            // Act
            var resultByName = _service.FilterPoliticalParties(politicalParties, "PoliticalParty1");

            // Assert
            var politicalPartiesList = resultByName.ToList();
            Assert.AreEqual(1, politicalPartiesList.Count);
            Assert.AreEqual("PoliticalParty1", politicalPartiesList[0].Name);
        }

        [Test]
        public void FilterPoliticalParties_ByIncorrectValues_ReturnsPoliticalPartiesWithoutError()
        {
            // Arrange
            var politicalParties = _mock.Object.FindAll();

            // Act
            var resultWithSpaces = _service.FilterPoliticalParties(politicalParties, "  PoliticalParty1   ");
            var resultWithNonexistentPoliticalParty = _service.FilterPoliticalParties(politicalParties, "NonexistentPoliticalParty");

            // Assert
            var politicalPartiesList = resultWithSpaces.ToList();
            Assert.AreEqual(1, politicalPartiesList.Count,
                "One political party should be listed with specified criteria.");
            Assert.AreEqual("PoliticalParty1", politicalPartiesList[0].Name,
                "Political party with specified criteria should be PoliticalParty1.");

            politicalPartiesList = resultWithNonexistentPoliticalParty.ToList();
            Assert.AreEqual(0, politicalPartiesList.Count,
                "No political party should be listed with specified criteria.");
        }
        #endregion
    }
}
