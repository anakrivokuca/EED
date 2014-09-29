using EED.Domain;
using EED.Service.Controller.Choices;
using EED.Ui.Web.Controllers;
using EED.Ui.Web.Models.Choices;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EED.Unit.Tests.Controllers
{
    [TestFixture]
    class ChoiceControllerTest
    {
        private Mock<IChoiceServiceController> _mock;
        private ChoiceController _controller;
        private IEnumerable<Choice> _choices;

        [SetUp]
        public void SetUp_ChoiceControllerTest()
        {
            // Arrange
            _choices = new List<Choice> { 
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
                    Project = new ElectionProject { Id = 2 }}};

            _mock = new Mock<IChoiceServiceController>();

            _mock.Setup(p => p.FindProject(1)).Returns(new ElectionProject
            {
                Id = 1,
                Contests = new List<Contest> { new Contest { Id = 1 }, 
                    new Contest { Id = 2, District = new District { Id = 1 } } },
                PoliticalParties = new List<PoliticalParty> { new PoliticalParty { Id = 1 }, 
                    new PoliticalParty { Id = 2 } },
                Choices = _choices.ToList()
            });

            _controller = new ChoiceController(_mock.Object);

            var session = new Mock<HttpSessionStateBase>();
            var context = new Mock<HttpContextBase>();
            session.SetupGet(s => s["projectId"]).Returns("1");
            context.SetupGet(x => x.Session).Returns(session.Object);
            _controller.ControllerContext = new ControllerContext(context.Object,
                new RouteData(), _controller);
        }

        #region Test List Method
        [Test]
        public void List_GivenThreeChoices_ReturnsThreeChoices()
        {
            // Act
            var result = ((ListViewModel)_controller.List(null).Model).ChoicesPerPage;

            // Assert
            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public void List_GivenThreeChoicesAndTwoChoicesPerPage_ReturnsOneChoiceOnTheSecondPage()
        {
            // Arrange
            _controller.ItemsPerPage = 2;

            // Act
            var result = ((ListViewModel)_controller.List(null, 0, 2).Model).ChoicesPerPage;

            // Assert
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void List_GetFilteredChoices_ReturnsOneChoice()
        {
            // Arrange
            var searchText = "Choice1";
            var contestId = 1;
            _mock.Setup(c => c.FilterChoices(_choices, searchText, contestId)).Returns(
                new List<Choice> {
                    new Choice { Id = 1, Name = "Choice1" }});

            // Act
            var result = ((ListViewModel)_controller.List(searchText, contestId).Model).ChoicesPerPage;

            // Assert
            Assert.AreEqual(1, result.Count());
        }
        #endregion

        #region Test Edit (Get) Method
        [Test]
        public void Edit_GetChoice_ReturnsCreateViewModel()
        {
            // Arrange
            var choiceId = 1;
            _mock.Setup(s => s.FindChoice(choiceId)).Returns(new Choice
            {
                Id = choiceId,
                Name = "Choice1",
                Contest = new Contest { Id = 1 },
                PoliticalParties = new List<PoliticalParty> { new PoliticalParty { Id = 2 }}
            });

            // Act
            var result = (CreateViewModel)_controller.Edit(choiceId).Model;

            // Assert
            Assert.AreEqual("Choice1", result.Name);
        }

        [Test]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void Edit_GetNonexistentChoice_ThrowsException()
        {
            // Arrange
            var choiceId = 101;
            Choice choice = null;
            _mock.Setup(s => s.FindChoice(choiceId)).Returns(choice);

            // Act
            var result = (CreateViewModel)_controller.Edit(choiceId).Model;

            // Assert
            Assert.IsNull(result);
        }
        #endregion
    }
}
