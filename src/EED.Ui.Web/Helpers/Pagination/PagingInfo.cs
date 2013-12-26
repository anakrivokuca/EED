using System;

namespace EED.Ui.Web.Helpers.Pagination
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