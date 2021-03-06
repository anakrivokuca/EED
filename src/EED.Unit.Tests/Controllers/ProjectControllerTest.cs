﻿using EED.Domain;
using EED.Service.Controller.Project;
using EED.Ui.Web.Controllers;
using EED.Ui.Web.Models.Project;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EED.Unit.Tests.Controllers
{
    [TestFixture]
    class ProjectControllerTest
    {
        private Mock<IProjectServiceController> _mock;
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

            _mock = new Mock<IProjectServiceController>();
            _mock.Setup(s => s.FindAllProjectsFromUser()).Returns(_projects);

            _controller = new ProjectController(_mock.Object);

            var session = new Mock<HttpSessionStateBase>();
            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Session).Returns(session.Object);
            _controller.ControllerContext = new ControllerContext(context.Object, 
                new RouteData(), _controller);
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
            // Arrange
            var projectId = 1;
            _mock.Setup(p => p.FindProject(projectId)).Returns(new ElectionProject
            {
                Id = projectId,
                Name = "Project1",
                ElectionType = new ElectionType { Id = 1 },
                JurisdictionType = new JurisdictionType { Id = 1 }
            });

            // Act
            var result = (CreateViewModel)_controller.Edit(projectId).Model;

            // Assert
            Assert.AreEqual("Project1", result.Name);
        }

        [Test]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void Edit_GetNonexistentProject_ThrowsException()
        {
            // Arrange
            var projectId = 101;
            ElectionProject project = null;
            _mock.Setup(p => p.FindProject(projectId)).Returns(project);

            // Act
            var result = (CreateViewModel)_controller.Edit(projectId).Model;

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
                Name = "NewProject", 
                Date = new DateTime(2012, 2, 8),
                ElectionTypeId =  1,
                JurisdictionTypeId = 1
            };

            // Act
            var result = _controller.Edit(model);

            // Assert
            _mock.Verify(m => m.SaveProject(It.IsAny<ElectionProject>()), Times.Once());
            Assert.IsNotNull(_controller.TempData["message-success"]);
            Assert.AreEqual("Project NewProject has been successfully saved.",
                _controller.TempData["message-success"]); 
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
        }

        [Test]
        public void Edit_PostExistingProjectWithValidChanges_ReturnsRedirectResult()
        {
            // Arrange
            var projectId = 2;
            var model = new CreateViewModel
            {
                Id = projectId,
                Name = "Project 2", 
                Date = new DateTime(2012, 2, 8),
                Description = "Project description.",
                JurisdictionTypeId = 1
            };

            // Act
            var result = _controller.Edit(model);

            // Assert
            _mock.Verify(m => m.SaveProject(It.IsAny<ElectionProject>()));
            Assert.IsNotNull(_controller.TempData["message-success"]);
            Assert.AreEqual("Project Project 2 has been successfully saved.",
                _controller.TempData["message-success"]); 
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
        public void Delete_GetValidProject_ReturnsSuccessMessage()
        {
            // Arrange
            var projectId = 1;
            var project = new ElectionProject { Id = projectId, Name = "Project1" };
            _mock.Setup(p => p.FindProject(projectId)).Returns(project);

            // Act
            _controller.Delete(projectId);

            // Assert
            _mock.Verify(m => m.DeleteProject(project), Times.Once());
            Assert.IsNotNull(_controller.TempData["message-success"]);
            Assert.AreEqual("Project " + project.Name + " has been successfully deleted.", 
                _controller.TempData["message-success"]); 
        }

        [Test]
        public void Delete_PostMultipleProjects_ReturnsSuccessMessage()
        {
            // Arrange
            var projects = new int[] { 1, 2, 3 };
            _mock.Setup(p => p.FindProject(projects[0])).Returns(new ElectionProject { Id = projects[0] });
            _mock.Setup(p => p.FindProject(projects[1])).Returns(new ElectionProject { Id = projects[1] });
            _mock.Setup(p => p.FindProject(projects[2])).Returns(new ElectionProject { Id = projects[2] });

            // Act
            _controller.Delete(projects);

            // Assert
            _mock.Verify(m => m.DeleteProject(It.IsAny<ElectionProject>()), Times.Exactly(projects.Count()));
            Assert.IsNotNull(_controller.TempData["message-success"]);
            Assert.AreEqual(projects.Count() + " project(s) has been successfully deleted.",
                _controller.TempData["message-success"]);
        }

        [Test]
        public void Delete_PostNoProject_ReturnsInfoMessage()
        {
            // Arrange
            int[] projects = null;

            // Act
            _controller.Delete(projects);

            // Assert
            _mock.Verify(m => m.DeleteProject(It.IsAny<ElectionProject>()), Times.Never);
            Assert.IsNotNull(_controller.TempData["message-info"]);
            Assert.AreEqual("None of the projects has been selected for delete action.",
                _controller.TempData["message-info"]);
        }
        #endregion

        #region Test Open Method
        [Test]
        public void Open_GetValidProject_ReturnsRedirectResult()
        {
            // Arrange
            var projectId = 1;
            
            // Act
            var result = _controller.Open(projectId);

            // Assert
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
        }
        #endregion

        #region Test Close Method
        [Test]
        public void Close_GetValidProject_ReturnsRedirectResult()
        {
            // Act
            var result = _controller.Close();

            // Assert
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
        }
        #endregion
    }
}
