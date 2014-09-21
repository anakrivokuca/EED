using EED.Domain;
using EED.Ui.Web.Helpers.Pagination;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace EED.Ui.Web.Models.Choices
{
    public class ListViewModel
    {
        public IEnumerable<Choice> ChoicesPerPage { get; set; }

        public PagingInfo PagingInfo { get; set; }

        [DisplayName("Name")]
        public string SearchText { get; set; }

        [DisplayName("Contest")]
        public int ContestId { get; set; }
        public IEnumerable<SelectListItem> Contests { get; set; }
    }
}