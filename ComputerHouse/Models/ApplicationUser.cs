using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ComputerHouse.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Display(Name ="First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Lockout Reason")]
        public string LockoutReason { get; set; }
    }
}