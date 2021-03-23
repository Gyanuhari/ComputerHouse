using System.Collections.Generic;

namespace ComputerHouse.Models.ViewModels
{
    public class BrandAndCategoryVM
    {
        public BrandCategory BrandCategory { get; set; }

        //For Dropdown
        public IEnumerable<Brand> BrandList { get; set; }

        public string StatusMessage { get; set; }
    }
}
