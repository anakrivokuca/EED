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
                    Office = new Office { Id = 1 },
                    District = new District { Id = 1, Name = "District1"}, 
                    Project = new ElectionProject { Id = 1 }},
                new Contest { Id = 2, Name = "Contest2", 
                    Office = new Office { Id = 1 },
                    District = new District { Id = 2, Name = "District2"}, 
                    Project = new ElectionProject { Id = 2 }},
                new Contest { Id = 3, Name = "Contest3", 
                    Office = new Office { Id = 2 },
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

        #region Test FilterContests Method
        [Test]
        public void FilterContests_ByNameAndOffice_ReturnsOneContest()
        {
            // Arrange
            var contests = _mock.Object.FindAll();
            var contestName = "Contest1";

            // Act
            var resultByName = _service.FilterContests(contests, contestName, 0);
            var resultByOffice = _service.FilterContests(contests, "", 1);

            // Assert
            var contestList = resultByName.ToList();
            Assert.AreEqual(1, contestList.Count());
            Assert.AreEqual(contestName, contestList[0].Name,
                "Contest with specified name should be " + contestName + ".");

            contestList = resultByOffice.ToList();
            Assert.AreEqual(2, contestList.Count());
            Assert.AreEqual("Contest1", contestList[0].Name,
                "First contest with specified office should be Contest1.");
            Assert.AreEqual("Contest2", contestList[1].Name,
                "Second contest with specified office should be Contest2.");
        }

        [Test]
        public void FilterContests_ByIncorrectValues_ReturnsContestsWithoutError()
        {
            // Arrange
            var contests = _mock.Object.FindAll();

            // Act
            var resultWithSpaces = _service.FilterContests(contests, "  Contest1   ", 0);
            var resultWithNonexistentContest = _service.FilterContests(contests, "NonexistentContest", 5);

            // Assert
            var contestsList = resultWithSpaces.ToList();
            Assert.AreEqual(1, contestsList.Count(),
                "One contest should be listed with specified criteria.");
            Assert.AreEqual("Contest1", contestsList[0].Name,
                "Contest with specified criteria should be Contest1.");

            Assert.AreEqual(0, resultWithNonexistentContest.Count(),
                "No contest should be listed with specified criteria.");
        }
        #endregion

        #region Test SaveContest Method
        [Test]
        public void SaveContest_NewValidContest_DoesNotThrowError()
        {
            // Arrange
            var contest = new Contest
            {
                Name = "NewContest",
                District = new District { Id = 1 },
                Office = new Office { Id = 1 }
            };

            // Act
            _service.SaveContest(contest);

            // Assert
            _mock.Verify(d => d.Save(contest), Times.Once());
        }
        #endregion

        #region Test DeleteContest Method
        [Test]
        public void DeleteContest_ValidContest_DoesNotThrowError()
        {
            // Arrange
            var contest = new Contest { Id = 1, Name = "Contest1" };

            // Act
            _service.DeleteContest(contest);

            // Assert
            _mock.Verify(m => m.Delete(It.IsAny<Contest>()), Times.Once());
        }
        #endregion
    }
}
