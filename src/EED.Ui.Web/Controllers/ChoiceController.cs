using EED.Domain;
using EED.Service.Controller.Choices;
using EED.Ui.Web.Filters;
using EED.Ui.Web.Helpers.Pagination;
using EED.Ui.Web.Models.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EED.Ui.Web.Controllers
{
    [Authorize]
    [SessionExpireFilter]
    public class ChoiceController : Controller
    {
        private readonly IChoiceServiceController _serviceController;

        private ElectionProject _project;

        public int ItemsPerPage = 10;

        public ChoiceController(IChoiceServiceController serviceController)
        {
            _serviceController = serviceController;
        }

        //
        // GET: /Choice/

        public ViewResult List(string searchText, int contestId = 0, int page = 1)
        {
            ViewBag.Title = "Choices";

            _project = GetProject();
            var choices = _project.Choices.AsEnumerable<Choice>();

            var contests = _project.Contests;
            var selectListContest = new SelectList(contests, "Id", "Name");

            if (contestId != 0 || !String.IsNullOrEmpty(searchText))
                choices = _serviceController.FilterChoices(choices, searchText, contestId).ToList();

            var pagingInfo = new PagingInfo()
            {
                CurrentPage = page,
                ItemsPerPage = ItemsPerPage,
                TotalNumberOfItems = choices.Count()
            };

            var choicesPerPage = choices
                .OrderBy(o => o.Name)
                .Skip((page - 1) * ItemsPerPage)
                .Take(ItemsPerPage);

            var model = new ListViewModel()
            {
                ChoicesPerPage = choicesPerPage,
                PagingInfo = pagingInfo,
                SearchText = searchText,
                Contests = selectListContest,
                ContestId = contestId
            };

            return View(model);
        }

        //
        // GET: /Choice/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Choice/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Choice/Create

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
        // GET: /Choice/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Choice/Edit/5

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
        // GET: /Choice/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Choice/Delete/5

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
