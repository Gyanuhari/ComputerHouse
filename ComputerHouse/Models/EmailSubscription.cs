using System.ComponentModel.DataAnnotations;

namespace ComputerHouse.Models
{
    public class EmailSubscription
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Enter Valid Email.")]
        public string Email { get; set; }


        public EmailSubscription()
        {

        }

        public EmailSubscription(string email)
        {
            this.Email = email;
        }
    }
}
