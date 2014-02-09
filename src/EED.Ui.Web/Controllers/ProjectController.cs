using EED.Domain;
using EED.Service.Election_Type;
using EED.Service.Jurisdiction_Type;
using EED.Service.Project;
using EED.Ui.Web.Helpers;
using EED.Ui.Web.Models.Project;
using System;
using System.Linq;
using System.Web.Mvc;

namespace EED.Ui.Web.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly IProjectService _service;
        private readonly IJurisdictionTypeService _jurisdictionTypeService;
        private readonly IElectionTypeService _electionTypeService;

        public ProjectController(IProjectService service, IJurisdictionTypeService jurisdictionTypeService, 
            IElectionTypeService electionTypeService)
        {
            _service = service;
            _jurisdictionTypeService = jurisdictionTypeService;
            _electionTypeService = electionTypeService;
        }

        //
        // GET: /Project/List

        public ViewResult List(string searchText)
        {
            var projects = _service.FindAllProjectsFromUser();

            if (!String.IsNullOrEmpty(searchText))
                projects = _service.FilterProjects(projects, searchText).ToList();

            var model = new ListViewModel() { 
                Projects = projects,
                SearchText = searchText
            };

            return View(model);
        }

        //
        // GET: /Project/Create

        public ViewResult Create()
        {
            var model = new CreateViewModel();
            model = PrepareModelToPopulateDropDownLists(model);

            return View("Edit", model);
        }

        //
        // GET: /Project/Edit/Id

        public ViewResult Edit(int id)
        {
            var project = _service.FindProject(id);

            var model = new CreateViewModel();
            model = model.ConvertProjectToModel(project);
            model = PrepareModelToPopulateDropDownLists(model);

            return View(model);
        }

        //
        // POST: /Project/Edit

        [HttpPost]
        public ActionResult Edit(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var project = model.ConvertModelToProject(model);
                if (model.Id != 0)
                {
                    var existingProject = _service.FindProject(model.Id);
                    project.JurisdictionType =  existingProject.JurisdictionType;
                    project.ElectionType = existingProject.ElectionType;
                }
                _service.SaveProject(project);
                TempData["message-success"] = string.Format(
                    "Project {0} has been successfully saved.", 
                    model.Name);

                return RedirectToAction("List");
            }
            else
            {
                return View(model);
            }
        }

        //
        // POST: /Project/Delete

        [HttpGet]
        //[HttpPost]
        //[MultipleButton(Name = "action", Argument = "Delete")]
        public ActionResult Delete(int id)
        {
            var project = new ElectionProject { Id = id };
            _service.DeleteProject(project);
            TempData["message-success"] = string.Format(
                "Project {0} has been successfully deleted.", id);

            return RedirectToAction("List");
        }

        //
        // POST: /Project/Delete

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "DeleteSelected")]
        public ActionResult Delete(int[] deleteInputs)
        {
            if (deleteInputs == null) {
                TempData["message-info"] = string.Format(
                    "None of the projects has been selected for delete action.");
                
                return RedirectToAction("List");
            }
            foreach (var id in deleteInputs)
            {
                var project = new ElectionProject { Id = id };
                _service.DeleteProject(project);
            }
            TempData["message-success"] = string.Format(deleteInputs.Count().ToString() + 
                " project(s) has been successfully deleted.");

            return RedirectToAction("List");
        }

        //
        // GET: /Project/Open/Id

        public ActionResult Open(int id)
        {
            var project = _service.FindProject(id);
            if (project != null)
            {
                Session["projectId"] = id;
                Session["projectName"] = project.Name;
            }
            
            return RedirectToAction("List", "DistrictType");
        }

        //
        // GET: /Project/Close/Id

        public ActionResult Close()
        {
            Session["projectId"] = null;
            Session["projectName"] = null;

            return RedirectToAction("List", "Project");
        }

        private CreateViewModel PrepareModelToPopulateDropDownLists(CreateViewModel model)
        {
            var jurisdictionTypes = _jurisdictionTypeService.FindAllJurisdictionTypes();
            var selectListJurisdictionType = new SelectList(jurisdictionTypes, "Id", "Name");

            var electionTypes = _electionTypeService.FindAllElectionTypes();
            var selectListElectionType = new SelectList(electionTypes, "Id", "Name");

            model.JurisdictionTypes = selectListJurisdictionType;
            model.ElectionTypes = selectListElectionType;

            return model;
        }
    }
}
