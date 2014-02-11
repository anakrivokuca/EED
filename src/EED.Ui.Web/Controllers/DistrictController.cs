using EED.Service.District;
using EED.Ui.Web.Filters;
using EED.Ui.Web.Models.District;
using System;
using System.Web.Mvc;

namespace EED.Ui.Web.Controllers
{
    [Authorize]
    [SessionExpireFilter]
    public class DistrictController : Controller
    {
        private readonly IDistrictService _service;

        public DistrictController(IDistrictService service)
        {
            _service = service;
        }

        //
        // GET: /District/

        public ViewResult List()
        {
            int projectId = Convert.ToInt32(Session["projectId"]);
            var districts = _service.FindAllDistrictsFromProject(projectId);

            var model = new ListViewModel()
            {
                Districts = districts
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
