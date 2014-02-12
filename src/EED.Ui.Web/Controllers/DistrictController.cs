using EED.Domain;
using EED.Service.District;
using EED.Service.District_Type;
using EED.Ui.Web.Filters;
using EED.Ui.Web.Helpers.Pagination;
using EED.Ui.Web.Models.District;
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
        private readonly IDistrictService _service;
        private readonly IDistrictTypeService _districtTypeService;

        public int ItemsPerPage = 10;

        public DistrictController(IDistrictService service, IDistrictTypeService districtTypeService)
        {
            _service = service;
            _districtTypeService = districtTypeService;
        }

        //
        // GET: /District/

        public ViewResult List(string searchText, int districtTypeId = 0, int page = 1)
        {
            int projectId = Convert.ToInt32(Session["projectId"]);
            var districts = _service.FindAllDistrictsFromProject(projectId);

            var districtTypes = _districtTypeService.FindAllDistrictTypesFromProject(projectId);
            var selectListDistrictType = new SelectList(districtTypes, "Id", "Name");

            if (districtTypeId != 0 || !String.IsNullOrEmpty(searchText))
                districts = _service.FilterDistricts(districts, searchText, districtTypeId);

            var pagingInfo = new PagingInfo()
            {
                CurrentPage = page,
                ItemsPerPage = ItemsPerPage,
                TotalNumberOfItems = districts.Count()
            };

            var districtsPerPage = districts
                .OrderBy(d => d.Id)
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
            var model = new CreateViewModel();
            model = PrepareModelToPopulateDropDownLists(model);

            return View("Edit", model);
        }

        //
        // GET: /District/Edit/Id

        public ViewResult Edit(int id)
        {
            var district = _service.FindDistrict(id);

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
                int projectId = Convert.ToInt32(Session["projectId"]);
                district.Project = new ElectionProject { Id = projectId };

                //if (model.Id != 0)
                //{
                //    var existingDistrict = _service.FindDistrict(model.Id);
                //    district.DistrictType = existingDistrict.DistrictType;
                //    district.ParentDistrict = existingDistrict.ParentDistrict;
                //}

                _service.SaveDistrict(district);
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
        // POST: /District/Delete/5

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
        
        [HttpPost]
        public ActionResult PopulateParentDistricts(int districtTypeId)
        {
            int projectId = Convert.ToInt32(Session["projectId"]);
            var districtTypes = GetDistrictTypes(projectId);
            var parentDistrictType = districtTypes.SingleOrDefault(dt => dt.Id == districtTypeId).ParentDistrictType;

            var districts = GetDistrictsForDistrictType(projectId, parentDistrictType.Id);
            var selectListDistrict = new SelectList(districts, "Id", "Name");

            return Json(selectListDistrict);
        }

        private CreateViewModel PrepareModelToPopulateDropDownLists(CreateViewModel model)
        {
            int projectId = Convert.ToInt32(Session["projectId"]);

            var districtTypes = GetDistrictTypes(projectId);
            var selectListDistrictType = new SelectList(districtTypes, "Id", "Name");
            model.DistrictTypes = selectListDistrictType;

            var districts = GetDistrictsForDistrictType(projectId, model.ParentDistrictId);
            var selectListDistrict = new SelectList(districts, "Id", "Name");
            model.ParentDistricts = selectListDistrict;

            return model;
        }

        private IEnumerable<DistrictType> GetDistrictTypes(int projectId)
        {
            var districtTypes = _districtTypeService.FindAllDistrictTypesFromProject(projectId).ToList();

            var jurisdictionType = districtTypes.SingleOrDefault(dt => dt.ParentDistrictType == null);
            districtTypes.Remove(jurisdictionType);

            return districtTypes;
        }

        private IEnumerable<District> GetDistrictsForDistrictType(int projectId, int districtTypeId)
        {
            var districts = _service.FindAllDistrictsFromProject(projectId);
            districts = districts.Where(d => d.DistrictType.Id == districtTypeId);

            return districts;
        }
    }
}
