using EED.Domain;
using EED.Service.Controller.Political_Party;
using EED.Ui.Web.Filters;
using EED.Ui.Web.Helpers.Pagination;
using EED.Ui.Web.Models.Political_Party;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EED.Ui.Web.Controllers
{
    [Authorize]
    [SessionExpireFilter]
    public class PoliticalPartyController : Controller
    {
        private readonly IPoliticalPartyServiceController _serviceController;

        private ElectionProject _project;

        public int ItemsPerPage = 10;

        public PoliticalPartyController(IPoliticalPartyServiceController serviceController)
        {
            _serviceController = serviceController;
        }

        //
        // GET: /PoliticalParty/

        public ViewResult List(string searchText, int page = 1)
        {
            ViewBag.Title = "PoliticalParties";

            _project = GetProject();
            var politicalParties = _project.PoliticalParties.AsEnumerable<PoliticalParty>();

            if (!String.IsNullOrEmpty(searchText))
                politicalParties = _serviceController.FilterPoliticalParties(politicalParties, searchText).ToList();

            var pagingInfo = new PagingInfo()
            {
                CurrentPage = page,
                ItemsPerPage = ItemsPerPage,
                TotalNumberOfItems = politicalParties.Count()
            };

            var politicalPartiesPerPage = politicalParties
                .OrderBy(o => o.Name)
                .Skip((page - 1) * ItemsPerPage)
                .Take(ItemsPerPage);

            var model = new ListViewModel()
            {
                PoliticalPartiesPerPage = politicalPartiesPerPage,
                SearchText = searchText,
                PagingInfo = pagingInfo
            };

            return View(model);
        }

        //
        // GET: /PoliticalParty/Create

        public ViewResult Create()
        {
            ViewBag.Title = "Add New Political Party";

            var model = new CreateViewModel();

            return View("Edit", model);
        }

        //
        // GET: /PoliticalParty/Edit/Id

        public ViewResult Edit(int id)
        {
            ViewBag.Title = "Edit";

            var politicalParty = _serviceController.FindPoliticalParty(id);

            var model = new CreateViewModel();
            model = model.ConvertPoliticalPartyToModel(politicalParty);

            return View(model);
        }

        //
        // POST: /PoliticalParty/Edit/5

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
        // GET: /PoliticalParty/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /PoliticalParty/Delete/5

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
