using EED.Domain;
using EED.Ui.Web.Helpers.Pagination;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace EED.Ui.Web.Models.Contests
{
    public class ListViewModel
    {
        public IEnumerable<Contest> ContestsPerPage { get; set; }

        public PagingInfo PagingInfo { get; set; }

        [DisplayName("Name")]
        public string SearchText { get; set; }

        [DisplayName("Office")]
        public int OfficeId { get; set; }
        public IEnumerable<SelectListItem> Offices { get; set; }
    }
}