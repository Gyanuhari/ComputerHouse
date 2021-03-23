using System;

namespace ComputerHouse.Models
{
    public class PagingInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }

        public PagingInfo(int totalItems, int itemsPerPage, int currentPage)
        {
            this.TotalItems = totalItems;
            this.ItemsPerPage = itemsPerPage;
            this.CurrentPage = currentPage;
        }

        //You can use like Convert.ToInt32(data)/ TryParse() is associated with string.
        //https://www.c-sharpcorner.com/article/uses-of-int-parse-convert-toint-and-int-tryparse/

        public int TotalPages => (int)((Math.Ceiling((decimal)TotalItems / ItemsPerPage)));
    }
}
