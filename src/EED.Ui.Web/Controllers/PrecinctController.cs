﻿using EED.Domain;
using EED.Service.Controller.Precincts;
using EED.Ui.Web.Filters;
using EED.Ui.Web.Helpers.Pagination;
using EED.Ui.Web.Models.Precincts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace EED.Ui.Web.Controllers
{
    [Authorize]
    [SessionExpireFilter]
    public class PrecinctController : Controller
    {
        private readonly IPrecinctServiceController _serviceController;
        public int ItemsPerPage = 10;

        public PrecinctController(IPrecinctServiceController serviceController)
        {
            _serviceController = serviceController;
        }

        //
        // GET: /Precinct/

        public ViewResult List(string searchText, int districtId = 0, int page = 1)
        {
            ViewBag.Title = "Precincts";

            int projectId = Convert.ToInt32(Session["projectId"]);
            var project = _serviceController.FindProject(projectId);
            var precincts = project.Precincts.AsEnumerable();

            var districts = project.Districts;
            var selectListDistrict = new SelectList(districts, "Id", "Name");

            if (districtId != 0 || !String.IsNullOrEmpty(searchText))
                precincts = _serviceController.FilterPrecincts(precincts, searchText, districtId);

            var pagingInfo = new PagingInfo()
            {
                CurrentPage = page,
                ItemsPerPage = ItemsPerPage,
                TotalNumberOfItems = precincts.Count()
            };

            var precinctsPerPage = precincts
                .OrderBy(d => d.Name)
                .Skip((page - 1) * ItemsPerPage)
                .Take(ItemsPerPage);

            var model = new ListViewModel()
            {
                PrecinctsPerPage = precinctsPerPage,
                PagingInfo = pagingInfo,
                SearchText = searchText,
                Districts = selectListDistrict,
                DistrictId = districtId
            };

            return View(model);
        }

        //
        // GET: /Precinct/Create

        public ActionResult Create()
        {
            ViewBag.Title = "Add New Precinct";

            var model = new CreateViewModel();
            model = PrepareModelToPopulateListBoxes(model);

            return View("Edit", model);
        }

        //
        // GET: /Precinct/Edit/Id

        public ViewResult Edit(int id)
        {
            ViewBag.Title = "Edit";

            var precinct = _serviceController.FindPrecinct(id);

            var model = new CreateViewModel();
            model = model.ConvertPrecinctToModel(precinct);
            model.SelectedDistricts = precinct.Districts.OrderBy(d => d.Name).ToList();
            model = PrepareModelToPopulateListBoxes(model);

            return View(model);
        }

        //
        // POST: /Precinct/Edit/Id

        [HttpPost]
        public ActionResult Edit(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                int projectId = Convert.ToInt32(Session["projectId"]);

                var precinct = model.ConvertModelToPrecinct(model);
                if (model.SelectedDistrictIds != null)
                {
                    var selectedDistricts = _serviceController.FindProject(projectId).Districts
                        .Where(d => model.SelectedDistrictIds.Contains(d.Id));
                    precinct.Districts = selectedDistricts.ToList();
                }
                precinct.Project = new ElectionProject { Id = projectId };

                _serviceController.SavePrecinct(precinct);
                TempData["message-success"] = string.Format(
                    "Precinct {0} has been successfully saved.",
                    model.Name);

                return RedirectToAction("List");
            }
            else
            {
                return View(model);
            }
        }

        //
        // GET: /Precinct/Delete/Id

        public ActionResult Delete(int id)
        {
            var precinct = _serviceController.FindPrecinct(id);
            _serviceController.DeletePrecinct(precinct);
            TempData["message-success"] = string.Format(
                "Precinct {0} has been successfully deleted.", precinct.Name);

            return RedirectToAction("List");
        }

        //
        // POST: /Precinct/Delete

        [HttpPost]
        public ActionResult Delete(int[] deleteInputs)
        {
            if (deleteInputs == null)
            {
                TempData["message-info"] = string.Format(
                    "None of the precincts has been selected for delete action.");

                return RedirectToAction("List");
            }
            foreach (var id in deleteInputs)
            {
                var precinct = _serviceController.FindPrecinct(id);
                _serviceController.DeletePrecinct(precinct);
            }
            TempData["message-success"] = string.Format(deleteInputs.Count().ToString() +
                " precinct(s) has been successfully deleted.");

            return RedirectToAction("List");
        }

        private CreateViewModel PrepareModelToPopulateListBoxes(CreateViewModel model)
        {
            int projectId = Convert.ToInt32(Session["projectId"]);

            model.Districts = _serviceController.FindProject(projectId).Districts.ToList();

            if (model.Id != 0)
            {
                model.SelectedDistrictIds = model.SelectedDistricts.Select(x => x.Id).ToArray();
                model.Districts = model.Districts.Where(d => !model.SelectedDistrictIds.Contains(d.Id))
                    .OrderBy(d => d.Name).ToList();
            }
            else
            {
                model.SelectedDistricts = new List<District>();
            }

            return model;
        }
    }
}
