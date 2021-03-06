﻿using EED.Domain;
using EED.Service.Controller.Districts;
using EED.Ui.Web.Filters;
using EED.Ui.Web.Helpers.Pagination;
using EED.Ui.Web.Models.Districts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace EED.Ui.Web.Controllers
{
    [Authorize]
    [SessionExpireFilter]
    public class DistrictController : Controller
    {
        private readonly IDistrictServiceController _serviceController;

        private ElectionProject _project;

        public int ItemsPerPage = 10;

        public DistrictController(IDistrictServiceController serviceController)
        {
            _serviceController = serviceController;
        }

        //
        // GET: /District/

        public ViewResult List(string searchText, int districtTypeId = 0, int page = 1)
        {
            ViewBag.Title = "Districts";

            _project = GetProject();
            var districts = _project.Districts.AsEnumerable<District>();

            var districtTypes = _project.DistrictTypes;
            var selectListDistrictType = new SelectList(districtTypes, "Id", "Name");

            if (districtTypeId != 0 || !String.IsNullOrEmpty(searchText))
                districts = _serviceController.FilterDistricts(districts, searchText, districtTypeId);

            var pagingInfo = new PagingInfo()
            {
                CurrentPage = page,
                ItemsPerPage = ItemsPerPage,
                TotalNumberOfItems = districts.Count()
            };

            var districtsPerPage = districts
                .OrderBy(d => d.Name)
                .Skip((page - 1) * ItemsPerPage)
                .Take(ItemsPerPage);

            var model = new ListViewModel()
            {
                DistrictsPerPage = districtsPerPage,
                PagingInfo = pagingInfo,
                SearchText = searchText,
                DistrictTypes = selectListDistrictType,
                DistrictTypeId = districtTypeId
            };

            return View(model);
        }

        //
        // GET: /District/Create

        public ViewResult Create()
        {
            ViewBag.Title = "Add New District";

            var model = new CreateViewModel();
            model = PrepareModelToPopulateDropDownLists(model);

            return View("Edit", model);
        }

        //
        // GET: /District/Edit/Id

        public ViewResult Edit(int id)
        {
            ViewBag.Title = "Edit";

            var district = _serviceController.FindDistrict(id);

            var model = new CreateViewModel();
            model = model.ConvertDistrictToModel(district);
            model = PrepareModelToPopulateDropDownLists(model);

            return View(model);
        }

        //
        // POST: /District/Edit

        [HttpPost]
        public ActionResult Edit(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var district = model.ConvertModelToDistrict(model);
                
                if (model.Id == 0)
                {
                    district.Project = GetProject();
                }
                _serviceController.SaveDistrict(district);
                
                TempData["message-success"] = string.Format(
                    "District {0} has been successfully saved.",
                    model.Name);

                return RedirectToAction("List");
            }
            else
            {
                return View(model);
            }
        }

        //
        // GET: /District/Delete/5

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var district = _serviceController.FindDistrict(id);
            _serviceController.DeleteDistrict(district);
            TempData["message-success"] = string.Format(
                "District {0} has been successfully deleted.", district.Name);

            return RedirectToAction("List");
        }

        //
        // POST: /District/Delete

        [HttpPost]
        public ActionResult Delete(int[] deleteInputs)
        {
            if (deleteInputs == null)
            {
                TempData["message-info"] = string.Format(
                    "None of the districts has been selected for delete action.");

                return RedirectToAction("List");
            }
            foreach (var id in deleteInputs)
            {
                var district = _serviceController.FindDistrict(id);
                _serviceController.DeleteDistrict(district);
            }
            TempData["message-success"] = string.Format(deleteInputs.Count().ToString() +
                " district(s) has been successfully deleted.");

            return RedirectToAction("List");
        }
        
        [HttpPost]
        public ActionResult PopulateParentDistricts(int districtTypeId)
        {
            _project = GetProject();

            var districtTypes = _project.DistrictTypes;
            var jurisdictionType = districtTypes
                .SingleOrDefault(dt => dt.ParentDistrictType == null);
            districtTypes.Remove(jurisdictionType);

            var parentDistrictType = districtTypes
                .SingleOrDefault(dt => dt.Id == districtTypeId).ParentDistrictType;

            var districts = GetDistrictsForDistrictType(parentDistrictType);
            var selectListDistrict = new SelectList(districts, "Id", "Name");

            return Json(selectListDistrict);
        }

        private CreateViewModel PrepareModelToPopulateDropDownLists(CreateViewModel model)
        {
            _project = GetProject();

            var districtTypes = _project.DistrictTypes;
            if (model.ParentDistrictId != 0 || model.Id == 0)
            {
                var jurisdictionType = districtTypes
                    .SingleOrDefault(dt => dt.ParentDistrictType == null);
                districtTypes.Remove(jurisdictionType);
            }
            var selectListDistrictType = new SelectList(districtTypes, "Id", "Name");
            model.DistrictTypes = selectListDistrictType;

            var districtType = districtTypes
                .SingleOrDefault(dt => dt.Id == model.DistrictTypeId);

            var districts = GetDistrictsForDistrictType(districtType);
            var selectListDistrict = new SelectList(districts, "Id", "Name");
            model.ParentDistricts = selectListDistrict;

            return model;
        }

        private IEnumerable<District> GetDistrictsForDistrictType(DistrictType districtType)
        {
            IEnumerable<District> districts = new List<District>();
            if(districtType != null)
                districts = districtType.Districts;

            return districts;
        }

        private ElectionProject GetProject()
        {
            int projectId = Convert.ToInt32(Session["projectId"]);
            return _serviceController.FindProject(projectId);
        }
    }
}
