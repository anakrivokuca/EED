using EED.Service.Controller.Precincts;
using EED.Ui.Web.Filters;
using EED.Ui.Web.Helpers.Pagination;
using EED.Ui.Web.Models.Precincts;
using System;
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
            var precincts = _serviceController.FindAllPrecinctsFromProject(projectId);

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
            return View();
        }

        //
        // GET: /Precinct/Edit/5

        public ViewResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Precinct/Edit/5

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
    }
}
