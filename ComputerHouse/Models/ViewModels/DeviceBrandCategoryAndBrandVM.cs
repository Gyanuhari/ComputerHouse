using System.Collections.Generic;

namespace ComputerHouse.Models.ViewModels
{
    public class DeviceBrandCategoryAndBrandVM
    {
        public Device Device { get; set; }
        public IEnumerable<OperatingSystem> OSList { get; set; }
        //Needed only in edit section
        public IEnumerable<BrandCategory> BrandCategoryList { get; set; }
        public IEnumerable<Brand> BrandList { get; set; }
        public string StatusMessage { get; set; }
    }
}
