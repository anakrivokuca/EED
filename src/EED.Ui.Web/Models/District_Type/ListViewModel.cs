using EED.Domain;
using System.Collections.Generic;

namespace EED.Ui.Web.Models.District_Type
{
    public class ListViewModel
    {
        public IEnumerable<DistrictType> DistrictTypes { get; set; }
        public string SearchText { get; set; }
    }
}