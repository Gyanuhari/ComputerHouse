using System.Collections.Generic;

namespace ComputerHouse.Models.ViewModels
{
    public class ShoppingListBrandListVM
    {
        public IList<Device> DeviceList { get; set; }
        public IList<Brand> BrandList { get; set; }
    }
}
