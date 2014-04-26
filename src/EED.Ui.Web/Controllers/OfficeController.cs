using EED.Domain;
using EED.Service.Controller.Offices;
using EED.Ui.Web.Filters;
using EED.Ui.Web.Helpers;
using EED.Ui.Web.Helpers.Pagination;
using EED.Ui.Web.Models.Offices;
using System;
using System.Linq;
using System.Web.Mvc;

namespace EED.Ui.Web.Controllers
{
    [Authorize]
    [SessionExpireFilter]
    public class OfficeController : Controller
    {
        private readonly IOfficeServiceController _serviceController;

        private ElectionProject _project;

        public int ItemsPerPage = 10;

        public OfficeController(IOfficeServiceController serviceController)
        {
            _serviceController = serviceController;
        }
        //
        // GET: /Office/

        public ViewResult List(string searchText, int page = 1)
        {
            ViewBag.Title = "Offices";

            _project = GetProject();
            var offices = _project.Offices.AsEnumerable<Office>();

            if (!String.IsNullOrEmpty(searchText))
                offices = _serviceController.FilterOffices(offices, searchText).ToList();

            var pagingInfo = new PagingInfo()
            {
                CurrentPage = page,
                ItemsPerPage = ItemsPerPage,
                TotalNumberOfItems = offices.Count()
            };

            var officesPerPage = offices
                .OrderBy(o => o.Name)
                .Skip((page - 1) * ItemsPerPage)
                .Take(ItemsPerPage);

            var model = new ListViewModel()
            {
                OfficesPerPage = officesPerPage,
                SearchText = searchText,
                PagingInfo = pagingInfo
            };

            return View(model);
        }

        //
        // GET: /Office/Create

        public ViewResult Create()
        {
            ViewBag.Title = "Add New Office";

            var model = new CreateViewModel() { NumberOfPositions = 1 };
            model = PrepareModelToPopulateDropDownLists(model);

            return View("Edit", model);
        }

        //
        // GET: /Office/Edit/Id

        public ViewResult Edit(int id)
        {
            ViewBag.Title = "Edit";

            var office = _serviceController.FindOffice(id);

            var model = new CreateViewModel();
            model = model.ConvertOfficeToModel(office);
            model = PrepareModelToPopulateDropDownLists(model);

            return View(model);
        }

        //
        // POST: /Office/Edit

        [HttpPost]
        public ActionResult Edit(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var office = model.ConvertModelToOffice(model);

                if (model.Id == 0)
                {
                    office.Project = GetProject();
                }
                _serviceController.SaveOffice(office);

                TempData["message-success"] = string.Format(
                    "Office {0} has been successfully saved.",
                    model.Name);

                return RedirectToAction("List");
            }
            else
            {
                return View(model);
            }
        }

        //
        // GET: /Office/Delete/5

        public ActionResult Delete(int id)
        {
            var office = _serviceController.FindOffice(id);
            _serviceController.DeleteOffice(office);
            TempData["message-success"] = string.Format(
                "Office {0} has been successfully deleted.", office.Name);

            return RedirectToAction("List");
        }

        //
        // POST: /Office/Delete/5

        [HttpPost]
        public ActionResult Delete(int[] deleteInputs)
        {
            if (deleteInputs == null)
            {
                TempData["message-info"] = string.Format(
                    "None of the offices has been selected for delete action.");

                return RedirectToAction("List");
            }
            foreach (var id in deleteInputs)
            {
                var office = _serviceController.FindOffice(id);
                _serviceController.DeleteOffice(office);
            }
            TempData["message-success"] = string.Format(deleteInputs.Count().ToString() +
                " office(s) has been successfully deleted.");

            return RedirectToAction("List");
        }

        private CreateViewModel PrepareModelToPopulateDropDownLists(CreateViewModel model)
        {
            _project = GetProject();

            var districtTypes = _project.DistrictTypes;
            
            var selectListDistrictType = new SelectList(districtTypes, "Id", "Name");
            model.DistrictTypes = selectListDistrictType;

            var districtType = districtTypes
                .SingleOrDefault(dt => dt.Id == model.DistrictTypeId);

            var officeTypes = Enum.GetValues(typeof(OfficeType));

            OfficeType officeType = OfficeType.Candidacy;
            foreach (OfficeType ot in officeTypes)
            {
                if ((int)ot == model.OfficeTypeId)
                {
                    officeType = ot;
                }
            }

            var selectListOfficeType = officeType.ToSelectList();
            model.OfficeTypes = selectListOfficeType;

            return model;
        }

        private ElectionProject GetProject()
        {
            int projectId = Convert.ToInt32(Session["projectId"]);
            return _serviceController.FindProject(projectId);
        }
    }
}
