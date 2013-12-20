using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EED.Domain;
using EED.Service;
using EED.Ui.Web.Pagination;

namespace EED.Ui.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _service;

        public int ItemsPerPage = 10;

        public UserController(IUserService service)
        {
            _service = service;
        }

        //
        // GET: /User/

        public ViewResult Users(int page = 1)
        {
            ViewData["Title"] = "Users";

            PagingInfo pagingInfo = new PagingInfo()
            {
                CurrentPage = page,
                ItemsPerPage = ItemsPerPage,
                TotalNumberOfItems = _service.FindAllUsers().Count()
            };
            ViewBag.PagingInfo = pagingInfo;

            var users = _service.FindAllUsers()
                .OrderBy(u => u.Id)
                .Skip((page - 1)*ItemsPerPage)
                .Take(ItemsPerPage);

            return View(users);
        }
    }
}
