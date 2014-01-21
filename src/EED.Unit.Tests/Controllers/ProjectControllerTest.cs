using EED.Domain;
using EED.Service.Project;
using EED.Ui.Web.Controllers;
using EED.Ui.Web.Models.Project;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EED.Unit.Tests.Controllers
{
    [TestFixture]
    class ProjectControllerTest
    {
        private Mock<IProjectService> _mock;
        private ProjectController _controller;
        private IEnumerable<ElectionProject> _projects;

        [SetUp]
        public void SetUp_ProjectControllerTest()
        {
            // Arrange
            _projects = new List<ElectionProject> {
                new ElectionProject { Id = 1, Name = "Project1" },
                new ElectionProject { Id = 2, Name = "Project2" }};

            _mock = new Mock<IProjectService>();
            _mock.Setup(s => s.FindAllProjectsFromUser()).Returns(_projects);

            _controller = new ProjectController(_mock.Object);
        }

        #region Test Projects Method
        [Test]
        public void Projects_GetTwoProjects_ReturnsTwoProjects()
        {
            // Act
            var result = ((ProjectsViewModel)_controller.Projects(null).Model).Projects;

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void Projects_GetFilteredProjects_ReturnsOneProject()
        {
            // Arrange
            var searchText = "Project1";
            _mock.Setup(p => p.FilterProjects(_projects, searchText)).Returns(
                new List<ElectionProject> {
                    new ElectionProject { Id = 1, Name = "Project1" }});

            // Act
            var result = ((ProjectsViewModel)_controller.Projects(searchText).Model).Projects;

            // Assert
            Assert.AreEqual(1, result.Count());
        }
        #endregion
    }
}
