using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EED.Service;

namespace EED.Ui.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _service = new UserService();

        //
        // GET: /User/

        public ViewResult Users()
        {
            return View(_service.FindAllUsers());
        }

    }
}
