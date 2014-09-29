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
        // GET: /Choice/Create

        public ActionResult Create()
        {
            ViewBag.Title = "Add New Choice";

            var model = new CreateViewModel();
            model = PrepareModelToPopulateDropDownLists(model);
            model = PrepareModelToPopulateListBoxes(model);

            return View("Edit", model);
        }

        //
        // GET: /Choice/Edit/5

        public ViewResult Edit(int id)
        {
            ViewBag.Title = "Edit";

            var choice = _serviceController.FindChoice(id);

            var model = new CreateViewModel();
            model = model.ConvertChoiceToModel(choice);
            model = PrepareModelToPopulateDropDownLists(model);
            model.SelectedPoliticalParties = choice.PoliticalParties.OrderBy(o => o.Name).ToList();
            model = PrepareModelToPopulateListBoxes(model);

            return View(model);
        }

        //
        // POST: /Choice/Edit/5

        [HttpPost]
        public ActionResult Edit(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                _project = GetProject();
                var choice = model.ConvertModelToChoice(model);

                if (model.SelectedPoliticalPartyIds != null)
                {
                    var selectedPoliticalParties = _project.PoliticalParties
                        .Where(o => model.SelectedPoliticalPartyIds.Contains(o.Id));
                    choice.PoliticalParties = selectedPoliticalParties.ToList();
                }

                if (model.Id == 0)
                {
                    choice.Project = _project;
                }
                _serviceController.SaveChoice(choice);

                TempData["message-success"] = string.Format(
                    "Choice {0} has been successfully saved.",
                    model.Name);

                return RedirectToAction("List");
            }
            else
            {
                return View(model);
            }
        }

        //
        // GET: /Choice/Delete/5

        public ActionResult Delete(int id)
        {
            var choice = _serviceController.FindChoice(id);
            _serviceController.DeleteChoice(choice);
            TempData["message-success"] = string.Format(
                "Choice {0} has been successfully deleted.", choice.Name);

            return RedirectToAction("List");
        }

        //
        // POST: /Choice/Delete/5

        [HttpPost]
        public ActionResult Delete(int[] deleteInputs)
        {
            if (deleteInputs == null)
            {
                TempData["message-info"] = string.Format(
                    "None of the choices has been selected for delete action.");

                return RedirectToAction("List");
            }
            foreach (var id in deleteInputs)
            {
                var choice = _serviceController.FindChoice(id);
                _serviceController.DeleteChoice(choice);
            }
            TempData["message-success"] = string.Format(deleteInputs.Count().ToString() +
                " choice(s) has been successfully deleted.");

            return RedirectToAction("List");
        }

        private ElectionProject GetProject()
        {
            int projectId = Convert.ToInt32(Session["projectId"]);
            return _serviceController.FindProject(projectId);
        }

        private CreateViewModel PrepareModelToPopulateDropDownLists(CreateViewModel model)
        {
            _project = GetProject();

            var contests = _project.Contests;
            var selectListContest = new SelectList(contests, "Id", "Name");
            model.Contests = selectListContest;

            var contest = contests
                .SingleOrDefault(o => o.Id == model.ContestId);

            return model;
        }

        private CreateViewModel PrepareModelToPopulateListBoxes(CreateViewModel model)
        {
            _project = GetProject();

            model.PoliticalParties =_project.PoliticalParties.ToList();

            if (model.Id != 0)
            {
                model.SelectedPoliticalPartyIds = model.SelectedPoliticalParties.Select(x => x.Id).ToArray();
                model.PoliticalParties = model.PoliticalParties
                    .Where(o => !model.SelectedPoliticalPartyIds.Contains(o.Id))
                    .OrderBy(o => o.Name).ToList();
            }
            else
            {
                model.SelectedPoliticalParties = new List<PoliticalParty>();
            }

            return model;
        }
    }
}
