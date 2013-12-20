using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EED.Ui.Web.Pagination
{
    public class PagingInfo
    {
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalNumberOfItems { get; set; }

        public int CalculateNumberOfPages()
        {
            return Convert.ToInt32((Math.Ceiling((decimal)TotalNumberOfItems / ItemsPerPage)));
        }
    }
}