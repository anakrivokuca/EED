using EED.Domain;
using EED.Ui.Web.Helpers.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EED.Ui.Web.Models.Political_Party
{
    public class ListViewModel
    {
        public IEnumerable<PoliticalParty> PoliticalPartiesPerPage { get; set; }
        public string SearchText { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}