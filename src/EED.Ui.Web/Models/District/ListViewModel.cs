using EED.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EED.Ui.Web.Models.District
{
    public class ListViewModel
    {
        public IEnumerable<Domain.District> Districts { get; set; }
    }
}