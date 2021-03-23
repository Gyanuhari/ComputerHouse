using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerHouse.Models
{
    public class WishList
    {
        public int Id { get; set; }

        [Required]
        public int DeviceId { get; set; }
        [ForeignKey("DeviceId")]
        public virtual Device Device { get; set; }

        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
