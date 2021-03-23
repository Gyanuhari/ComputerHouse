using System;
using System.ComponentModel.DataAnnotations;

namespace ComputerHouse.Models.ViewModels
{
    public class RecentOrderViewModel
    {
        [Required(ErrorMessage ="Please Enter Order Number!")]
        [Range(1,int.MaxValue, ErrorMessage ="{0} should be minimum {1} and at max {2}.")]
        [Display(Name ="Order Number")]
        public int OrderNo { get; set; }

        public string OrderStatus { get; set; }

        public DateTime ExpectedArrival { get; set; }

        public string StatusMessage { get; set; }
    }
}
