using EED.Domain;
using EED.Service.Controller.Offices;
using EED.Ui.Web.Filters;
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

        public ViewResult List(int page = 1)
        {
            ViewBag.Title = "Offices";

            _project = GetProject();
            var offices = _project.Offices.AsEnumerable<Office>();

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
                PagingInfo = pagingInfo
            };

            return View(model);
        }

        //
        // GET: /Office/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Office/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Office/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Office/Edit/5

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
        // GET: /Office/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Office/Delete/5

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

        private ElectionProject GetProject()
        {
            int projectId = Convert.ToInt32(Session["projectId"]);
            return _serviceController.FindProject(projectId);
        }
    }
}
