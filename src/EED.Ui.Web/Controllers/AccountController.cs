using EED.Infrastructure;
using EED.Service.Membership_Provider;
using EED.Ui.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EED.Ui.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthProvider _authProvider;

        public AccountController(IAuthProvider authProvider)
        {
            _authProvider = authProvider;
        }

        //
        // GET: /Account/Login

        public ViewResult Login()
        {
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (_authProvider.Authenticate(model.Username, model.Password))
                {
                    if (model.Username == "admin")
                        return Redirect(returnUrl ?? Url.Action("Users", "User"));
                    else
                        return Redirect(returnUrl ?? Url.Action("Projects", "Project"));
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
            var user = _authProvider.GetUserFromCookie();
            _authProvider.Logout(user);

            return RedirectToAction("Login", "Account");
        }
    }
}
