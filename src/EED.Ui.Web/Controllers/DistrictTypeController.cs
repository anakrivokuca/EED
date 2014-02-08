using EED.Service.District_Type;
using EED.Ui.Web.Filters;
using EED.Ui.Web.Models.District_Type;
using System;
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

        public ActionResult Create()
        {
            return View();
        }

        //
        // GET: /DistrictType/Edit

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /DistrictType/Edit

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("List");
            }
            catch
            {
                return View();
            }
        }

        //
        // POST: /DistrictType/Delete/

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("List");
            }
            catch
            {
                return View();
            }
        }
    }
}
