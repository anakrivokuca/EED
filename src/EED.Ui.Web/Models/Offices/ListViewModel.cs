using EED.Domain;
using EED.Ui.Web.Helpers.Pagination;
using System.Collections.Generic;

namespace EED.Ui.Web.Models.Offices
{
    public class ListViewModel
    {
        public IEnumerable<Office> OfficesPerPage { get; set; }
        public string SearchText { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}