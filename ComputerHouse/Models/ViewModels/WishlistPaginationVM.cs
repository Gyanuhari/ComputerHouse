using System.Collections.Generic;

namespace ComputerHouse.Models.ViewModels
{
    public class WishlistPaginationVM
    {
        public IList<WishList> WishLists { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
