using EED.Domain;
using EED.Ui.Web.Helpers.Pagination;
using System.Collections.Generic;

namespace EED.Ui.Web.Models.Contests
{
    public class ListViewModel
    {
        public IEnumerable<Contest> ContestsPerPage { get; set; }
        public string SearchText { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}