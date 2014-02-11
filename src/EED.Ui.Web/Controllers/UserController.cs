using EED.Domain;
using EED.Infrastructure;
using EED.Service.Membership_Provider;
using EED.Ui.Web.Helpers.Pagination;
using EED.Ui.Web.Models.User;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace EED.Ui.Web.Controllers
{
    [Authorize(Users = "admin")]
    public class UserController : Controller
    {
        public IMembershipProvider _provider;
        public int ItemsPerPage = 10;

        public UserController()
        {
            DependencyResolver.SetResolver(new NinjectDependencyResolver());
            _provider = DependencyResolver.Current.GetService<IMembershipProvider>();
            _provider.Initialize("", new NameValueCollection());
            //_provider = (CustomMembershipProvider)Membership.Providers["CustomMembershipProvider"];
        }

        //
        // GET: /User/List

        public ViewResult List(string searchText, int page = 1)
        {
            ViewBag.Title = "Users";
            
            var users = _provider.GetAllUsers().ToList();
            if (!String.IsNullOrEmpty(searchText))
                users = _provider.FilterUsers(users, searchText).ToList();

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
            return View("Edit", new CreateViewModel());
        }

        //
        // GET: /User/Edit/Id

        public ViewResult Edit(int id)
        {
            var user = _provider.GetAllUsers()
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
                User existingUser = _provider.GetAllUsers().SingleOrDefault
                    (u => u.UserName == model.UserName);

                var user = model.ConvertModelToUser(model);
                var status = new MembershipCreateStatus();
                
                if (existingUser == null)
                    user = _provider.CreateUser(user, out status);
                else
                    _provider.UpdateUser(user);
                
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
        public ActionResult Delete(int id, string name, string surname, string username)
        {
            var user = new User { Id = id };
            _provider.DeleteUser(username, true);
            TempData["message-success"] = string.Format("User {0} {1} has been successfully deleted.", 
                name, surname);
            
            return RedirectToAction("List");
        }
    }
}
