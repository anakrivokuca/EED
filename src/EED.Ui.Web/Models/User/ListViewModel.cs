using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EED.Domain;
using EED.Ui.Web.Helpers;
using EED.Ui.Web.Helpers.Pagination;

namespace EED.Ui.Web.Models
{
    public class UsersListViewModel
    {
        public IEnumerable<User> Users { get; set; }
        public PagingInfo PagingInfo { get; set; }
        //public UserSearchCriteria UserSearchCriteria { get; set; }
        public string SearchText { get; set; }
    }
}