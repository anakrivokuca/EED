using EED.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EED.Ui.Web.Models.Project
{
    public class ListViewModel
    {
        public IEnumerable<ElectionProject> Projects { get; set; }
        public string SearchText { get; set; }
    }
}