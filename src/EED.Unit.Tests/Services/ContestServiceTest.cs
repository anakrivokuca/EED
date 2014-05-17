using EED.DAL;
using EED.Domain;
using EED.Service.Contests;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace EED.Unit.Tests.Services
{
    [TestFixture]
    class ContestServiceTest
    {
        private Mock<IRepository<Contest>> _mock;
        private IContestService _service;

        [SetUp]
        public void SetUp_ContestServiceTest()
        {
            // Arrange
            _mock = new Mock<IRepository<Contest>>();
            _mock.Setup(d => d.FindAll()).Returns(new List<Contest> { 
                new Contest { Id = 1, Name = "Contest1", 
                    District = new District { Id = 1, Name = "District1"}, 
                    Project = new ElectionProject { Id = 1 }},
                new Contest { Id = 2, Name = "Contest2", 
                    District = new District { Id = 2, Name = "District2"}, 
                    Project = new ElectionProject { Id = 2 }},
                new Contest { Id = 3, Name = "Contest3", 
                    District = new District { Id = 2, Name = "District2"}, 
                    Project = new ElectionProject { Id = 2 }}});

            _service = new ContestService(_mock.Object);
        }

        #region Test FindAllContests Method
        [Test]
        public void FindAllContests_GivenThreeContests_ReturnsThreeContests()
        {
            // Act
            var result = _service.FindAllContests();

            // Assert
            Assert.AreEqual(3, result.Count());
        }
        #endregion
    }
}
