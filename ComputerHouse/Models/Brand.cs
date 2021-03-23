using System;
using System.ComponentModel.DataAnnotations;

namespace ComputerHouse.Models
{
    public class Brand
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name="Brand Name")]
        [MinLength(4, ErrorMessage ="Name should have atleast 4 characters.")]
        [MaxLength(60,ErrorMessage ="Name should be less than 60 characters.")]
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        //We will come to add display oreder and createdBy, Update At, UpdatedBy(Application User) later if we need
    }
}
