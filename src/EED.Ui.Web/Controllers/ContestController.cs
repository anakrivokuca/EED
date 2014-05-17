using EED.Domain;
using EED.Service.Controller.Contests;
using EED.Ui.Web.Filters;
using EED.Ui.Web.Helpers.Pagination;
using EED.Ui.Web.Models.Contests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EED.Ui.Web.Controllers
{
    [Authorize]
    [SessionExpireFilter]
    public class ContestController : Controller
    {
        private readonly IContestServiceController _serviceController;

        private ElectionProject _project;

        public int ItemsPerPage = 10;

        public ContestController(IContestServiceController serviceController)
        {
            _serviceController = serviceController;
        }

        //
        // GET: /Contest/

        public ViewResult List(string searchText, int officeId = 0, int page = 1)
        {
            ViewBag.Title = "Contests";

            _project = GetProject();
            var contests = _project.Contests.AsEnumerable<Contest>();

            var offices = _project.Offices;
            var selectListOffice = new SelectList(offices, "Id", "Name");

            if (officeId != 0 || !String.IsNullOrEmpty(searchText))
                contests = _serviceController.FilterContests(contests, searchText, officeId).ToList();

            var pagingInfo = new PagingInfo()
            {
                CurrentPage = page,
                ItemsPerPage = ItemsPerPage,
                TotalNumberOfItems = contests.Count()
            };

            var contestsPerPage = contests
                .OrderBy(o => o.Name)
                .Skip((page - 1) * ItemsPerPage)
                .Take(ItemsPerPage);

            var model = new ListViewModel()
            {
                ContestsPerPage = contestsPerPage,
                PagingInfo = pagingInfo,
                SearchText = searchText,
                Offices = selectListOffice,
                OfficeId = officeId
            };

            return View(model);
        }

        //
        // GET: /Contest/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Contest/Create

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
        // GET: /Contest/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Contest/Edit/5

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
        // GET: /Contest/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Contest/Delete/5

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
