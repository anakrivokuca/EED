using EED.Domain;
using EED.Service.District_Type;
using EED.Ui.Web.Filters;
using EED.Ui.Web.Models.District_Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace EED.Ui.Web.Controllers
{
    [Authorize]
    [SessionExpireFilter]
    public class DistrictTypeController : Controller
    {
        private readonly IDistrictTypeService _service;

        public DistrictTypeController(IDistrictTypeService service)
        {
            _service = service;
        }

        //
        // GET: /DistrictType/

        public ViewResult List(string searchText)
        {
            int projectId = Convert.ToInt32(Session["projectId"]);
            var districtTypes = _service.FindAllDistrictTypesFromProject(projectId);

            if (!String.IsNullOrEmpty(searchText))
                districtTypes = _service.FilterDistrictTypes(districtTypes, searchText).ToList();

            var model = new ListViewModel() {
                DistrictTypes = districtTypes,
                SearchText = searchText
            };

            return View(model);
        }

        //
        // GET: /DistrictType/Create

        public ViewResult Create()
        {
            var model = new CreateViewModel();
            model = PrepareModelToPopulateDropDownLists(model);

            return View("Edit", model);
        }

        //
        // GET: /DistrictType/Edit/Id

        public ViewResult Edit(int id)
        {
            var districtType = _service.FindDistrictType(id);

            var model = new CreateViewModel();
            model = model.ConvertDistrictTypeToModel(districtType);
            model = PrepareModelToPopulateDropDownLists(model);

            return View(model);
        }

        //
        // POST: /DistrictType/Edit

        [HttpPost]
        public ActionResult Edit(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var districtType = model.ConvertModelToDistrictType(model);
                int projectId = Convert.ToInt32(Session["projectId"]);
                districtType.Project = new ElectionProject { Id = projectId };

                _service.SaveDistrictType(districtType);
                TempData["message-success"] = string.Format(
                    "District type {0} has been successfully saved.",
                    model.Name);

                return RedirectToAction("List");
            }
            else
            {
                return View(model);
            }
        }

        //
        // POST: /DistrictType/Delete/

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var districtType = _service.FindDistrictType(id);
            _service.DeleteDistrictType(districtType);
            TempData["message-success"] = string.Format(
                "District type {0} has been successfully deleted.", districtType.Name);

            return RedirectToAction("List");
        }

        private CreateViewModel PrepareModelToPopulateDropDownLists(CreateViewModel model)
        {
            int projectId = Convert.ToInt32(Session["projectId"]);
            var districtTypes = _service.FindAllDistrictTypesFromProject(projectId);
            if (model.Id != 0)
            {
                districtTypes = RemoveSelectedDistrictTypeFromParentsList(model.Id, districtTypes);
            }
            var selectListDistrictType = new SelectList(districtTypes, "Id", "Name");

            model.DistrictTypes = selectListDistrictType;

            return model;
        }

        private IEnumerable<DistrictType> RemoveSelectedDistrictTypeFromParentsList(int id,
            IEnumerable<DistrictType> districtTypes)
        {
            var districtTypeList = districtTypes.ToList();
            var districtType = districtTypeList.SingleOrDefault(dt => dt.Id == id);
            districtTypeList.Remove(districtType);
            
            return districtTypeList;
        }
    }
}
