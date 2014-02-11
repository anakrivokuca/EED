using EED.Domain;
using EED.Ui.Web.Helpers.Pagination;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace EED.Ui.Web.Models.District
{
    public class ListViewModel
    {
        public IEnumerable<Domain.District> DistrictsPerPage { get; set; }

        public PagingInfo PagingInfo { get; set; }

        [DisplayName("Name")]
        public string SearchText { get; set; }

        [DisplayName("District Type")]
        public int DistrictTypeId { get; set; }
        public IEnumerable<SelectListItem> DistrictTypes { get; set; }
    }
}