using EED.Domain;
using System.Collections.Generic;

namespace EED.Ui.Web.Models.Project
{
    public class ListViewModel
    {
        public IEnumerable<ElectionProject> Projects { get; set; }
        public string SearchText { get; set; }
    }
}