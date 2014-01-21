using EED.Domain;
using EED.Service.Project;
using EED.Ui.Web.Models.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace EED.Ui.Web.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly IProjectService _service;

        public ProjectController(IProjectService service)
        {
            _service = service;
        }

        //
        // GET: /ElectionProject/

        public ViewResult Projects(string searchText)
        {
            var projects = _service.FindAllProjectsFromUser();

            if (!String.IsNullOrEmpty(searchText))
                projects = _service.FilterProjects(projects, searchText).ToList();

            var model = new ProjectsViewModel() { 
                Projects = projects,
                SearchText = searchText
            };

            return View(model);
        }
    }
}
