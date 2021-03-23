using System.Collections.Generic;

namespace ComputerHouse.Models.ViewModels
{
    public class HomeIndexViewModel
    {
        public IList<Device> NewProductList { get; set; }
        public EmailSubscription EmailSubscription { get; set; }
    }
}
