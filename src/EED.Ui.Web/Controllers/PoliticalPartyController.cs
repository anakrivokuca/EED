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

        public FileContentResult ShowImage(int id)
        {
            var politicalParty = _serviceController.FindPoliticalParty(id);

            if (politicalParty != null && politicalParty.Image != null)
            {
                return File(politicalParty.Image, politicalParty.Name);
            }
            return null;
        }

        //
        // POST: /PoliticalParty/Edit/5

        [HttpPost]
        public ActionResult Edit(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                var politicalParty = model.ConvertModelToPoliticalParty(model);

                if (model.Id == 0)
                {
                    politicalParty.Project = GetProject();
                }
                _serviceController.SavePoliticalParty(politicalParty);

                TempData["message-success"] = string.Format(
                    "Political party {0} has been successfully saved.",
                    model.Name);

                return RedirectToAction("List");
            }
            else
            {
                return View(model);
            }
        }

        //
        // GET: /PoliticalParty/Delete/5

        public ActionResult Delete(int id)
        {
            var politicalParty = _serviceController.FindPoliticalParty(id);
            _serviceController.DeletePoliticalParty(politicalParty);
            TempData["message-success"] = string.Format(
                "Political party {0} has been successfully deleted.", politicalParty.Name);

            return RedirectToAction("List");
        }

        //
        // POST: /PoliticalParty/Delete

        [HttpPost]
        public ActionResult Delete(int[] deleteInputs)
        {
            if (deleteInputs == null)
            {
                TempData["message-info"] = string.Format(
                    "None of the political parties has been selected for delete action.");

                return RedirectToAction("List");
            }
            foreach (var id in deleteInputs)
            {
                var politicalParty = _serviceController.FindPoliticalParty(id);
                _serviceController.DeletePoliticalParty(politicalParty);
            }
            TempData["message-success"] = string.Format(deleteInputs.Count().ToString() +
                " political party(s) has been successfully deleted.");

            return RedirectToAction("List");
        }

        private ElectionProject GetProject()
        {
            int projectId = Convert.ToInt32(Session["projectId"]);
            return _serviceController.FindProject(projectId);
        }
    }
}
