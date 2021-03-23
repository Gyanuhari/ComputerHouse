using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerHouse.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        [Required]
        public int DeviceId { get; set; }

        //[NotMapped]
        [ForeignKey("DeviceId")]
        public Device Device { get; set; }

        public int ItemCount { get; set; } = 1;
    }
}
