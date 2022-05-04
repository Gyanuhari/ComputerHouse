using System.Collections.Generic;

namespace ComputerHouse.Models.ViewModels
{
    public class ShoppingListVM
    {
        public IList<Device> DeviceList { get; set; }
        public string SearchName { get; set; }
    }
}
