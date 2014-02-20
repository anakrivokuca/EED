using EED.Domain;
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

        public ViewResult List(int page = 1)
        {
            int projectId = Convert.ToInt32(Session["projectId"]);
            var precincts = _serviceController.FindProject(projectId).Precincts;

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
                Precincts = precinctsPerPage,
                PagingInfo = pagingInfo
            };

            return View(model);
        }

        //
        // GET: /Precinct/Create

        public ActionResult Create()
        {
            var model = new CreateViewModel();
            model = PrepareModelToPopulateListBoxes(model);

            return View("Edit", model);
        }

        //
        // GET: /Precinct/Edit/Id

        public ViewResult Edit(int id)
        {
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
                    var districts = AddParentDistricts(model, selectedDistricts);
                    precinct.Districts = districts.ToList();
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
        // GET: /Precinct/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Precinct/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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

        private IEnumerable<District> AddParentDistricts(CreateViewModel model, IEnumerable<District> selectedDistricts)
        {
            var districts = selectedDistricts;

            while (selectedDistricts.Count() != 0)
            {
                var parentDistricts = selectedDistricts
                    .Where(sd => sd.ParentDistrict != null &&
                        !model.SelectedDistrictIds.Contains(sd.ParentDistrict.Id))
                    .Select(sd => sd.ParentDistrict);

                districts = districts.Concat(parentDistricts);
                selectedDistricts = parentDistricts;
                model.SelectedDistrictIds = districts.Select(x => x.Id).ToArray();
            }

            return districts;
        }
    }
}
