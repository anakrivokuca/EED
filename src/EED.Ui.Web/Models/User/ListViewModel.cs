using EED.Domain;
using EED.Ui.Web.Helpers.Pagination;
using System.Collections.Generic;

namespace EED.Ui.Web.Models.User
{
    public class ListViewModel
    {
        public IEnumerable<Domain.User> Users { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string SearchText { get; set; }
    }
}