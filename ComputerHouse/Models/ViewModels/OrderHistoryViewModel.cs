using System.Collections.Generic;

namespace ComputerHouse.Models.ViewModels
{
    public class OrderHistoryViewModel
    {
        public IList<OrderDetail> OrderDetailList { get; set; }
        public IList<OrderHeader> OrderHeaderList { get; set; }
    }
}
