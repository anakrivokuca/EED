using EED.DAL;
using EED.Domain;
using EED.Service.Choices;
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
    class ChoiceServiceTest
    {
        private Mock<IRepository<Choice>> _mock;
        private IChoiceService _service;

        [SetUp]
        public void SetUp_ChoiceServiceTest()
        {
            // Arrange
            _mock = new Mock<IRepository<Choice>>();
            _mock.Setup(d => d.FindAll()).Returns(new List<Choice> { 
                new Choice { Id = 1, Name = "Choice1", 
                    Contest = new Contest { Id = 1 },
                    PoliticalParties = new List<PoliticalParty> { 
                        new PoliticalParty { Id = 1, Name = "PoliticalParty1"}}, 
                    Project = new ElectionProject { Id = 1 }},
                new Choice { Id = 2, Name = "Choice2", 
                    Contest = new Contest { Id = 1 },
                    PoliticalParties = new List<PoliticalParty> { 
                        new PoliticalParty { Id = 1, Name = "PoliticalParty1"}, 
                        new PoliticalParty { Id = 2, Name = "PoliticalParty2"}}, 
                    Project = new ElectionProject { Id = 2 }},
                new Choice { Id = 3, Name = "Choice3", 
                    Contest = new Contest { Id = 2 }, 
                    Project = new ElectionProject { Id = 2 }}});

            _service = new ChoiceService(_mock.Object);
        }

        #region Test FindAllChoices Method
        [Test]
        public void FindAllChoices_GivenThreeChoices_ReturnsThreeChoices()
        {
            // Act
            var result = _service.FindAllChoices();

            // Assert
            Assert.AreEqual(3, result.Count());
        }
        #endregion

        #region Test FilterChoices Method
        [Test]
        public void FilterChoices_ByNameAndContest_ReturnsOneChoice()
        {
            // Arrange
            var choices = _mock.Object.FindAll();
            var choiceName = "Choice1";

            // Act
            var resultByName = _service.FilterChoices(choices, choiceName, 0);
            var resultByContest = _service.FilterChoices(choices, "", 1);

            // Assert
            var choiceList = resultByName.ToList();
            Assert.AreEqual(1, choiceList.Count());
            Assert.AreEqual(choiceName, choiceList[0].Name,
                "Choice with specified name should be " + choiceName + ".");

            choiceList = resultByContest.ToList();
            Assert.AreEqual(2, choiceList.Count());
            Assert.AreEqual("Choice1", choiceList[0].Name,
                "First choice with specified contest should be Choice1.");
            Assert.AreEqual("Choice2", choiceList[1].Name,
                "Second choice with specified contest should be Choice2.");
        }

        [Test]
        public void FilterChoices_ByIncorrectValues_ReturnsChoicesWithoutError()
        {
            // Arrange
            var choices = _mock.Object.FindAll();

            // Act
            var resultWithSpaces = _service.FilterChoices(choices, "  Choice1   ", 0);
            var resultWithNonexistentChoice = _service.FilterChoices(choices, "NonexistentChoice", 5);

            // Assert
            var choicesList = resultWithSpaces.ToList();
            Assert.AreEqual(1, choicesList.Count(),
                "One choice should be listed with specified criteria.");
            Assert.AreEqual("Choice1", choicesList[0].Name,
                "Choice with specified criteria should be Choice1.");

            Assert.AreEqual(0, resultWithNonexistentChoice.Count(),
                "No choice should be listed with specified criteria.");
        }
        #endregion
    }
}
