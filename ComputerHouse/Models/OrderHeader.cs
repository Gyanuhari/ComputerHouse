using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerHouse.Models
{
    public class OrderHeader
    {
        public int Id { get; set; }

        //For Someone who is going to pickup the order
        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Invalid Phone Number")]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.EmailAddress,ErrorMessage ="Please Enter Valid Email.")]
        public string Email { get; set; }

        [Required]
        [Display(Name="Street Address")]
        public string StreetAddress { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required, Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        //To keep the record of the one who made the order
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required, Display(Name = "Order Total")]
        public double OrderTotal { get; set; }

        //If Coupon is used, if admin edits the coupons we can still view the coupon used
        public string CouponCode { get; set; }

        public string Status { get; set; }

        public string Comments { get; set; }
    }
}