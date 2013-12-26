using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using EED.Domain;
using EED.Service;
using EED.Ui.Web.Helpers.Pagination;
using EED.Ui.Web.Models;

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

        public ViewResult Users(string searchText, int page = 1)
        {
            ViewBag.Title = "Users";
            
            var users = _service.FindAllUsers().ToList();
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
                        String.Equals(u.Username, keyword, StringComparison.CurrentCultureIgnoreCase)));
            }
            return users;
        }
    }
}
