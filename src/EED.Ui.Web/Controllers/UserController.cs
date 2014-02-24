using EED.Domain;
using EED.Service.Controller.User;
using EED.Ui.Web.Helpers.Pagination;
using EED.Ui.Web.Models.User;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace EED.Ui.Web.Controllers
{
    [Authorize(Users = "admin")]
    public class UserController : Controller
    {
        public IUserServiceController _serviceController;
        public int ItemsPerPage = 10;

        public UserController(IUserServiceController serviceController)
        {
            _serviceController = serviceController;
        }

        //
        // GET: /User/List

        public ViewResult List(string searchText, int page = 1)
        {
            ViewBag.Title = "Users";
            
            var users = _serviceController.GetAllUsers().ToList();
            if (!String.IsNullOrEmpty(searchText))
                users = _serviceController.FilterUsers(users, searchText).ToList();

            var pagingInfo = new PagingInfo()
            {
                CurrentPage = page,
                ItemsPerPage = ItemsPerPage,
                TotalNumberOfItems = users.Count()
            };

            var usersPerPage = users
                .OrderBy(u => u.Id)
                .Skip((page - 1)*ItemsPerPage)
                .Take(ItemsPerPage);

            var model = new ListViewModel()
            {
                Users = usersPerPage,
                PagingInfo = pagingInfo,
                SearchText = searchText
            };

            return View(model);
        }

        //
        // GET: /User/Create

        public ViewResult Create()
        {
            ViewBag.Title = "Add New User";

            return View("Edit", new CreateViewModel());
        }

        //
        // GET: /User/Edit/Id

        public ViewResult Edit(int id)
        {
            ViewBag.Title = "Edit";

            var user = _serviceController.GetAllUsers()
                .First(u => u.Id == id);
            var model = new CreateViewModel();
            model = model.ConvertUserToModel(user);
            return View(model);
        }

        //
        // POST: /User/Edit

        [HttpPost]
        public ActionResult Edit(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                User existingUser = _serviceController.GetUser(model.UserName);

                var user = model.ConvertModelToUser(model);
                var status = new MembershipCreateStatus();
                
                if (existingUser == null)
                    user = _serviceController.CreateUser(user, out status);
                else
                    _serviceController.UpdateUser(user);
                
                if (user == null)
                {
                    ModelState.AddModelError("",
                        "User is not saved. " + status);
                    return View(model);
                }
                else
                {
                    TempData["message-success"] = string.Format(
                        "User {0} {1} has been successfully saved.", model.Name, model.Surname);
                    return RedirectToAction("List");
                }
            }
            else
            {
                return View(model);
            }
        }

        //
        // POST: /User/Delete

        [HttpPost]
        public ActionResult Delete(string username)
        {
            var user = _serviceController.GetUser(username);
            _serviceController.DeleteUser(username, true);
            TempData["message-success"] = string.Format("User {0} {1} has been successfully deleted.", 
                user.Name, user.Surname);
            
            return RedirectToAction("List");
        }
    }
}
