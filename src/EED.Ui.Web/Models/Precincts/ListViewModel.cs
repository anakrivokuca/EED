using EED.Domain;
using EED.Ui.Web.Helpers.Pagination;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace EED.Ui.Web.Models.Precincts
{
    public class ListViewModel
    {
        public IEnumerable<Precinct> PrecinctsPerPage { get; set; }
        public PagingInfo PagingInfo { get; set; }

        [DisplayName("Name")]
        public string SearchText { get; set; }

        [DisplayName("District")]
        public int DistrictId { get; set; }
        public IEnumerable<SelectListItem> Districts { get; set; }
    }
}