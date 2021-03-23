using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerHouse.Models
{
    public class BrandCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name ="Brand's Category")]
        [MinLength(4, ErrorMessage = "Name should have atleast 5 characters.")]
        [MaxLength(50, ErrorMessage = "Name should be less than 50 characters")]
        public string Name { get; set; }

        [Required]
        [Display(Name="Brand")]
        public int BrandId { get; set; }

        [ForeignKey("BrandId")]
        public Brand Brand { get; set; }
    }
}
