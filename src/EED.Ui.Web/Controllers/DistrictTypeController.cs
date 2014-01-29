using EED.Ui.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EED.Ui.Web.Controllers
{
    [SessionExpireFilter]
    public class DistrictTypeController : Controller
    {
        //
        // GET: /DistrictType/

        public ActionResult List()
        {
            return View();
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

                return RedirectToAction("Index");
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

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
