using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerHouse.Models
{
    public class Device
    {
        public int Id { get; set; }

        [Required]
        [Display(Name="Device Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Hard Disk Capacity")]  
        public int HDCapacity { get; set; }

        public enum EHDCapacity
        {
            [Display(Name = "32 Gegabytes")]
            _32GB =1,

            [Display(Name = "64 Gegabytes")]
            _64GB =2,

            [Display(Name = "128 Gegabytes")]
            _128GB =3,

            [Display(Name = "256 Gegabytes")]
            _256GB =4,

            [Display(Name = "512 Gegabytes")]
            _512GB =5,

            [Display(Name = "1 Terabyte")]
            _1024GB =6
        }

        [Required]
        [Display(Name = "RAM")]                 
        public int RAMCapacity { get; set; }

        public enum ERAMCapacity
        {
            [Display(Name = "1 Gegabyte")]
            _1GB = 1,

            [Display(Name = "2 Gegabytes")]
            _2GB = 2,

            [Display(Name = "4 Gegabyte")]
            _4GB = 3,

            [Display(Name = "8 Gegabyte")]
            _8GB = 4,

            [Display(Name = "16 Gegabyte")]
            _16GB = 5,

            [Display(Name = "32 Gegabyte")]
            _32GB = 6
        }

        [Required]
        [Range(1,5000,ErrorMessage ="This cannot be a valid cost prize.")]
        public double Price { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Bluetooth Support")]   
        public bool Bluetooth { get; set; }

        [Required]
        [Range(4,34,ErrorMessage ="Size should be between 4-34 inches.")]
        [Display(Name = "Screen Size")]
        public double ScreenSize { get; set; }

        //[Required]
        public byte[] Image { get; set; }

        public enum EHDTypes
        {
            [Display(Name = "Hard Disk Drives")]
            HDD = 1,
            [Display(Name = "Solid State Drive")]
            SSD = 2
        }

        [Display(Name = "Hard Disk")]
        public int HDType { get; set; }

        [Display(Name = "HDMI Support")]        
        public bool HDMI { get; set; }

        [Display(Name = "Touch Screen")]        
        public bool IsTouchScreen { get; set; }

        [Display(Name = "Is New")]              
        public bool IsNew { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Display(Name="Item Rated")]
        public int Rated { get; set; }

        [Required]
        [Display(Name="Operating System")]
        public int OSId { get; set; }
        [ForeignKey("OSId")]
        public OperatingSystem OperatingSystem { get; set; }

        [Required]
        [Display(Name="Brand Category")]
        public int BrandCategoryId { get; set; }
        [ForeignKey("BrandCategoryId")]
        public BrandCategory BrandCategory { get; set; }

        [Required]
        [Display(Name = "Brand")]
        public int BrandId { get; set; }
        [ForeignKey("BrandId")]
        public Brand Brand { get; set; }
    }
}
