using EED.DAL;
using EED.Domain;
using EED.Service.Membership_Provider;
using EED.Service.Project;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EED.Unit.Tests.Services
{
    [TestFixture]
    class ProjectServiceTest
    {
        private Mock<IRepository<ElectionProject>> _mock;
        private Mock<IAuthProvider> _mockProvider;
        private IProjectService _service;
        private IEnumerable<ElectionProject> _projects;

        [SetUp]
        public void SetUp_ProjectServiceTest()
        {
            _mock = new Mock<IRepository<ElectionProject>>();
            _mock.Setup(r => r.FindAll()).Returns(new List<ElectionProject> {
                new ElectionProject { Id = 1, Name = "Project1", 
                    JurisdictionName = "Jurisdiction1", User = new User { Id = 1} },
                new ElectionProject { Id = 2, Name = "Project2", 
                    JurisdictionName = "Jurisdiction2", User = new User { Id = 1} },
                new ElectionProject { Id = 3, Name = "Project3", 
                    JurisdictionName = "Jurisdiction1", User = new User { Id = 2} }});

            _mockProvider = new Mock<IAuthProvider>();
            _mockProvider.Setup(p => p.GetUserFromCookie()).Returns(
                new User { Id = 3, UserName = "sarah" });

            _service = new ProjectService(_mock.Object, _mockProvider.Object);
        }

        #region Test FindAllProjects Method
        [Test]
        public void FindAllProjects_GivenThreeProjects_ResturnsThreeProjects()
        {
            // Act
            var result = _service.FindAllProjects();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
        }
        #endregion

        #region Test FindAllProjectsFromUser Method
        [Test]
        public void FindAllProjectsFromUser_GivenTwoProjectsForSpecifiedUser_ReturnsTwoProjects()
        {
            // Arrange
            _mockProvider.Setup(p => p.GetUserFromCookie()).Returns(
                new User { Id = 1, UserName = "ana" });

            // Act
            var result = _service.FindAllProjectsFromUser();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void FindAllProjectsFromUser_GivenZeroProjectsForSpecifiedUser_ReturnsZeroProjects()
        {
            // Act
            var result = _service.FindAllProjectsFromUser();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }
        #endregion

        #region Test FilterProjects Method
        [Test]
        public void FilterProjects_ByMultipleCriteria_ReturnsDifferentProjects()
        {
            // Arrange
            _projects = _mock.Object.FindAll();

            // Act
            var resultByName = _service.FilterProjects(_projects, "Project1");
            var resultByNameAndJurisdiction = _service.FilterProjects(_projects, "Project1 Jurisdiction1");
            var resultByJurisdictionName = _service.FilterProjects(_projects, "Jurisdiction1");

            // Assert
            var projects = resultByName.ToList();
            Assert.AreEqual(1, projects.Count,
                "One project should be listed with the specified name.");
            Assert.AreEqual("Project1", projects[0].Name,
                "Project with specified name should be Project1.");

            projects = resultByNameAndJurisdiction.ToList();
            Assert.AreEqual(1, projects.Count,
                "One project should be listed with specified name and jurisdiction.");
            Assert.AreEqual("Project1", projects[0].Name,
                "Project with specified name and jurisdiction should be Project1.");

            projects = resultByJurisdictionName.ToList();
            Assert.AreEqual(2, projects.Count,
                "Two projects should be listed with specified jurisdiction name.");
            Assert.AreEqual("Project1", projects[0].Name,
                "First project with specified jurisdiction should be Project1.");
            Assert.AreEqual("Project3", projects[1].Name,
                "Second project with specified jurisdiction should be Project3.");
        }

        [Test]
        public void FilterProjects_ByIncorrectValues_ReturnsProjectsWithoutError()
        {
            // Arrange
            _projects = _mock.Object.FindAll();

            // Act
            var resultWithSpaces = _service.FilterProjects(_projects,
                "  Project1   Jurisdiction1 ");
            var resultWithNonexistentProject = _service.FilterProjects(_projects, "NonexistentProject");
            var resultWithKeywordsFromDifferentProjects = _service.FilterProjects(_projects, "Project1 Jurisdiction2");

            // Assert
            var projects = resultWithSpaces.ToList();
            Assert.AreEqual(1, projects.Count,
                "One project should be listed with specified criteria.");
            Assert.AreEqual("Project1", projects[0].Name,
                "Project with all criteria specified should be Project1.");

            projects = resultWithNonexistentProject.ToList();
            Assert.AreEqual(0, projects.Count,
                "No project should be listed with specified criteria.");

            projects = resultWithKeywordsFromDifferentProjects.ToList();
            Assert.AreEqual(0, projects.Count,
                "No project should be listed with specified criteria.");
        }
        #endregion

        #region Test SaveProject Method
        [Test]
        public void SaveProject_NewValidProject_DoesNotThrowError()
        {
            // Arrange
            var project = new ElectionProject
            {
                Id = 100,
                Name = "NewProject",
                Date = new DateTime(2012, 6, 5),
                JurisdictionName = "CountyName",
                JurisdictionType = new JurisdictionType { Id = 1 },
                ElectionType = new ElectionType { Id = 1 }
            };

            // Act
            _service.SaveProject(project);

            // Assert
            _mock.Verify(m => m.Save(project));
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SaveProject_WithInvalidDate_ThrowsException()
        {
            // Arrange
            var project = new ElectionProject
            {
                Id = 100,
                Name = "ProjectWithInvalidDate",
                Date = new DateTime(2012, 2012, 5)
            };

            // Act
            _service.SaveProject(project);

            // Assert
            _mock.Verify(m => m.Save(project), Times.Never());
        }
        #endregion

        #region Test DeleteProject Method
        [Test]
        public void DeleteProject_ValidProject_DoesNotThrowError()
        {
            // Arrange
            var project = new ElectionProject { Id = 1, Name = "Project1" };

            // Act
            _service.DeleteProject(project);

            // Assert
            _mock.Verify(m => m.Delete(It.IsAny<ElectionProject>()));
        }
        #endregion
    }
}
