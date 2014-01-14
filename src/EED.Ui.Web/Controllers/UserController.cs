﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.WebPages;
using EED.Domain;
using EED.Service.Membership_Provider;
using EED.Ui.Web.Helpers.Pagination;
using EED.Ui.Web.Models;
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
           _provider =
                (CustomMembershipProvider)Membership.Providers["CustomMembershipProvider"];
        }

        //
        // GET: /User/Users

        public ViewResult Users(string searchText, int page = 1)
        {
            ViewBag.Title = "Users";
            
            var users = _provider.GetAllUsers().ToList();
            if (!String.IsNullOrEmpty(searchText))
                users = FilterUsers(users, searchText).ToList();

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

            var usersListViewModel = new UsersListViewModel()
            {
                Users = usersPerPage,
                PagingInfo = pagingInfo,
                SearchText = searchText
            };

            return View(usersListViewModel);
        }

        public IEnumerable<User> FilterUsers(IEnumerable<User> users, string searchText)
        {
            string[] keywords = searchText.Trim().Split(' ');
            foreach (var k in keywords.Where(k => !k.IsEmpty()))
            {
                string keyword = k;
                users = users
                    .Where(u => (String.Equals(u.Name, keyword, StringComparison.CurrentCultureIgnoreCase) ||
                        String.Equals(u.Surname, keyword, StringComparison.CurrentCultureIgnoreCase) ||
                        String.Equals(u.Email, keyword, StringComparison.CurrentCultureIgnoreCase) ||
                        String.Equals(u.State, keyword, StringComparison.CurrentCultureIgnoreCase) ||
                        String.Equals(u.Country, keyword, StringComparison.CurrentCultureIgnoreCase) ||
                        String.Equals(u.UserName, keyword, StringComparison.CurrentCultureIgnoreCase)));
            }
            return users;
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
                    TempData["message"] = string.Format("User {0} {1} has been successfully saved.",
                                model.Name, model.Surname);
                    return RedirectToAction("Users");
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
            TempData["message"] = string.Format("User {0} {1} has been successfully deleted.", 
                name, surname);
            
            return RedirectToAction("Users");
        }
    }
}
