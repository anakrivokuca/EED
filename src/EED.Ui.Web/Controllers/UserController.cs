using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EED.Domain;
using EED.Service;

namespace EED.Ui.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        //
        // GET: /User/

        public ViewResult Users()
        {
            ViewData["Title"] = "Users";
            return View(_service.FindAllUsers());
        }
    }
}
