﻿using EED.Service.Controller.Account;
using EED.Ui.Web.Models.User;
using System.Web.Mvc;

namespace EED.Ui.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountServiceController _serviceController;

        public AccountController(IAccountServiceController serviceController)
        {
            _serviceController = serviceController;
        }

        //
        // GET: /Account/Login

        public ViewResult Login()
        {
            ViewBag.Title = "Login";
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (_serviceController.Authenticate(model.Username, model.Password))
                {
                    if (model.Username == "admin")
                        return Redirect(returnUrl ?? Url.Action("List", "User"));
                    else
                        return Redirect(returnUrl ?? Url.Action("List", "Project"));
                }
                else
                {
                    ModelState.AddModelError("", 
                        "Login failed. The username or password is incorrect. Please try again.");
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        //
        // POST: /Account/LogOut

        [HttpPost]
        public ActionResult Logout()
        {
            var user = _serviceController.GetUserFromCookie();
            _serviceController.Logout(user);
            Session["projectId"] = null;
            Session["projectName"] = null;

            return RedirectToAction("Login", "Account");
        }
    }
}
