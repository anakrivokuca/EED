using EED.Service.District;
using EED.Service.District_Type;
using EED.Ui.Web.Filters;
using EED.Ui.Web.Helpers.Pagination;
using EED.Ui.Web.Models.District;
using System;
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

        public ActionResult Create()
        {
            return View();
        }

        //
        // GET: /District/Edit/Id

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /District/Edit

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
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
    }
}
