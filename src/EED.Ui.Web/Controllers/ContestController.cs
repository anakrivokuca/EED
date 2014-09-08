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
            ViewBag.Title = "Add New Contest";

            var model = new CreateViewModel();
            model = PrepareModelToPopulateDropDownLists(model);

            return View("Edit", model);
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
        // GET: /Contest/Edit/Id

        public ViewResult Edit(int id)
        {
            ViewBag.Title = "Edit";

            var contest = _serviceController.FindContest(id);

            var model = new CreateViewModel();
            model = model.ConvertContestToModel(contest);
            model = PrepareModelToPopulateDropDownLists(model);

            return View(model);
        }

        //
        // POST: /Contest/Edit/Id

        [HttpPost]
        public ActionResult Edit(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var contest = model.ConvertModelToContest(model);

                if (model.Id == 0)
                {
                    contest.Project = GetProject();
                }
                _serviceController.SaveContest(contest);

                TempData["message-success"] = string.Format(
                    "Contest {0} has been successfully saved.",
                    model.Name);

                return RedirectToAction("List");
            }
            else
            {
                return View(model);
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

        [HttpPost]
        public ActionResult PopulateDistricts(int officeId)
        {
            _project = GetProject();

            var offices = _project.Offices;
            var office = offices.SingleOrDefault(o => o.Id == officeId);

            var districts = GetDistrictsForOffice(office);
            var selectListDistrict = new SelectList(districts, "Id", "Name");

            return Json(selectListDistrict);
        }

        private CreateViewModel PrepareModelToPopulateDropDownLists(CreateViewModel model)
        {
            _project = GetProject();

            var offices = _project.Offices;

            var selectListOffice = new SelectList(offices, "Id", "Name");
            model.Offices = selectListOffice;

            var office = offices
                .SingleOrDefault(o => o.Id == model.OfficeId);

            IEnumerable<District> districts = GetDistrictsForOffice(office);

            var selectListDistrict = new SelectList(districts, "Id", "Name");
            model.Districts = selectListDistrict;

            return model;
        }

        private IEnumerable<District> GetDistrictsForOffice(Office office)
        {
            IEnumerable<District> districts;
            if (office == null)
            {
                districts = _project.Districts;
            }
            else
            {
                districts = _project.Districts.Where(d => d.DistrictType == office.DistrictType);
            }
            return districts;
        }

        private ElectionProject GetProject()
        {
            int projectId = Convert.ToInt32(Session["projectId"]);
            return _serviceController.FindProject(projectId);
        }
    }
}
