using EED.Domain;
using EED.Service.Election_Type;
using EED.Service.Jurisdiction_Type;
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
                new ElectionProject { Id = 1, Name = "Project1", JurisdictionName = "County1",
                    JurisdictionType = new JurisdictionType { Id = 1 },
                    ElectionType = new ElectionType { Id = 1 }},
                new ElectionProject { Id = 2, Name = "Project2", JurisdictionName = "County2",
                    JurisdictionType = new JurisdictionType { Id = 1 },
                    ElectionType = new ElectionType { Id = 1 } }};

            _mock = new Mock<IProjectService>();
            _mock.Setup(s => s.FindAllProjectsFromUser()).Returns(_projects);

            var _mockJurisdictionType = new Mock<IJurisdictionTypeService>();
            _mockJurisdictionType.Setup(s => s.FindAllJurisdictionTypes()).Returns(new List<JurisdictionType> { 
                new JurisdictionType { Id = 1, Name = "County"},
                new JurisdictionType { Id = 1, Name = "Municipality"}});

            var _mockElectionType = new Mock<IElectionTypeService>();
            _mockElectionType.Setup(s => s.FindAllElectionTypes()).Returns(new List<ElectionType> { 
                new ElectionType { Id = 1, Name = "General Elections"},
                new ElectionType { Id = 1, Name = "General Elections"}});

            _controller = new ProjectController(_mock.Object, _mockJurisdictionType.Object, 
                _mockElectionType.Object);
        }

        #region Test List Method
        [Test]
        public void List_GivenTwoProjects_ReturnsTwoProjects()
        {
            // Act
            var result = ((ListViewModel)_controller.List(null).Model).Projects;

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void List_GetFilteredProjects_ReturnsOneProject()
        {
            // Arrange
            var searchText = "Project1";
            _mock.Setup(p => p.FilterProjects(_projects, searchText)).Returns(
                new List<ElectionProject> {
                    new ElectionProject { Id = 1, Name = "Project1" }});

            // Act
            var result = ((ListViewModel)_controller.List(searchText).Model).Projects;

            // Assert
            Assert.AreEqual(1, result.Count());
        }
        #endregion

        #region Test Edit (Get) Method
        [Test]
        public void Edit_GetProject_ReturnsCreateViewModel()
        {
            // Act
            var result1 = (CreateViewModel)_controller.Edit(1).Model;
            var result2 = (CreateViewModel)_controller.Edit(2).Model;

            // Assert
            Assert.AreEqual("Project1", result1.Name,
                "Selected project should be Project1.");
            Assert.AreEqual("Project2", result2.Name,
                "Selected project should be Project2.");
        }

        [Test]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void Edit_GetNonexistentProject_ThrowsException()
        {
            // Act
            var result = (CreateViewModel)_controller.Edit(101).Model;

            // Assert
            Assert.IsNull(result);
        }
        #endregion

        #region Test Edit (Post) Method
        [Test]
        public void Edit_PostNewProject_ReturnsRedirectResult()
        {
            // Arrange
            var model = new CreateViewModel
            {
                Id = 100,
                Name = "NewProject", 
                Date = new DateTime(2012, 2, 8),
                ElectionTypeId =  1,
                JurisdictionTypeId = 1
            };
            var project = model.ConvertModelToProject(model);

            // Act
            var result = _controller.Edit(model);

            // Assert
            _mock.Verify(m => m.SaveProject(It.IsAny<ElectionProject>()), Times.Once());
            Assert.AreEqual("Project NewProject has been successfully saved.",
                _controller.TempData["message"]); 
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
        }

        [Test]
        public void Edit_PostExistingProjectWithValidChanges_ReturnsRedirectResult()
        {
            // Arrange
            var model = new CreateViewModel
            {
                Id = 2,
                Name = "Project 2", 
                Date = new DateTime(2012, 2, 8),
                Description = "Project description.",
                JurisdictionTypeId = 1
            };
            var project = model.ConvertModelToProject(model);

            // Act
            var result = _controller.Edit(model);

            // Assert
            _mock.Verify(m => m.SaveProject(It.IsAny<ElectionProject>()));
            Assert.AreEqual("Project Project 2 has been successfully saved.",
                _controller.TempData["message"]); 
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
        }

        [Test]
        public void Edit_PostExistingProjectWithInvalidChanges_ReturnsViewResult()
        {
            // Arrange
            var model = new CreateViewModel { 
                Name = "Project2", 
                Date = new DateTime(2012, 2, 8) 
            };
            var project = model.ConvertModelToProject(model);
            _controller.ModelState.AddModelError("error", "error");

            // Act
            var result = _controller.Edit(model);

            // Assert
            _mock.Verify(m => m.SaveProject(project), Times.Never());
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }
        #endregion

        #region Test Delete Method
        [Test]
        public void Delete_PostValidProject_ReturnsActionResult()
        {
            // Arrange
            var project = new ElectionProject { Id = 1, Name = "Project1" };

            // Act
            _controller.Delete(project.Id, project.Name);

            // Assert
            _mock.Verify(m => m.DeleteProject(It.IsAny<ElectionProject>()), Times.Once());
            Assert.AreEqual("Project Project1 has been successfully deleted.", 
                _controller.TempData["message"]); 
        }
        #endregion
    }
}
