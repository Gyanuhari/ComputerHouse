using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerHouse.Models
{
    public class CustomerContactUs
    {
        public int Id { get; set; }

        [Required]
        [Display(Name="Name")]
        //[MinLength(2, ErrorMessage = "Name should be greater than 2 characters.")]
        //[MaxLength(50, ErrorMessage ="Name should be less than 50 characters.")]
        [StringLength(50,ErrorMessage ="{0} must be minimum {1} and at max {2} characters long.", MinimumLength =2)]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress,ErrorMessage ="Please Enter Valid Email.")]
        public string Email { get; set; }

        [Required]
        [Display(Name ="Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Please enter valid phone number.")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Mobile Number.")]
        public string PhoneNumber{ get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        [StringLength(255)]
        public string Message { get; set; }

        //To upload picture by support for happycustomer display
        public byte[] Picture { get; set; }

        public bool IsHappyCustomer { get; set; }

        public bool Display { get; set; }

        //If in case we need more data related to contacting person.
        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
