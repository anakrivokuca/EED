using EED.Domain;
using EED.Ui.Web.Helpers.Pagination;
using System.Collections.Generic;

namespace EED.Ui.Web.Models.Precincts
{
    public class ListViewModel
    {
        public IEnumerable<Precinct> Precincts { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}