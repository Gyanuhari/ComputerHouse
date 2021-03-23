using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerHouse.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }

        [Required]
        public int OrderHeaderId { get; set; }

        [ForeignKey("OrderHeaderId")]
        public OrderHeader OrderHeader { get; set; }

        [Required]
        public int DeviceId { get; set; }

        [ForeignKey("DeviceId")]
        public virtual Device Device { get; set; }

        ////If incase someone edits the Name and Price the record in orderdetails woun't change
        //[Required]
        //public string MenuName { get; set; }

        [Required]
        public double SubTotal { get; set; }

        [Required]
        public int ItemCount { get; set; }
    }
}
