﻿using EED.Domain;
using EED.Service.Controller.Project;
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
        private readonly IProjectServiceController _serviceController;

        public ProjectController(IProjectServiceController serviceController)
        {
            _serviceController = serviceController;
        }

        //
        // GET: /Project/List

        public ViewResult List(string searchText)
        {
            ViewBag.Title = "Projects";

            var projects = _serviceController.FindAllProjectsFromUser();

            if (!String.IsNullOrEmpty(searchText))
                projects = _serviceController.FilterProjects(projects, searchText).ToList();

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
            ViewBag.Title = "Add New Project";

            var model = new CreateViewModel();
            model = PrepareModelToPopulateDropDownLists(model);

            return View("Edit", model);
        }

        //
        // GET: /Project/Edit/Id

        public ViewResult Edit(int id)
        {
            ViewBag.Title = "Edit";

            var project = _serviceController.FindProject(id);

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

                _serviceController.SaveProject(project);
                
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
            var project = _serviceController.FindProject(id);
            _serviceController.DeleteProject(project);
            TempData["message-success"] = string.Format(
                "Project {0} has been successfully deleted.", project.Name);

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
                var project = _serviceController.FindProject(id);
                _serviceController.DeleteProject(project);
            }
            TempData["message-success"] = string.Format(deleteInputs.Count().ToString() + 
                " project(s) has been successfully deleted.");

            return RedirectToAction("List");
        }

        //
        // GET: /Project/Open/Id

        public ActionResult Open(int id)
        {
            var project = _serviceController.FindProject(id);
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
            var jurisdictionTypes = _serviceController.FindAllJurisdictionTypes();
            var selectListJurisdictionType = new SelectList(jurisdictionTypes, "Id", "Name");

            var electionTypes = _serviceController.FindAllElectionTypes();
            var selectListElectionType = new SelectList(electionTypes, "Id", "Name");

            model.JurisdictionTypes = selectListJurisdictionType;
            model.ElectionTypes = selectListElectionType;

            return model;
        }
    }
}
