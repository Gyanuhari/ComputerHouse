using System.ComponentModel.DataAnnotations;

namespace ComputerHouse.Models
{
    public class OperatingSystem
    {
        public int Id { get; set; }

        [Required]
        [Display(Name="Operating System")]
        public string Name { get; set; }
    }
}
