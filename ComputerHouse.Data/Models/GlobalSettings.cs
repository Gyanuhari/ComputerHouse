using System.ComponentModel.DataAnnotations;
using System;

namespace ComputerHouse.Data.Models
{
    public class GlobalSettings
    {
        // Urls of the hero images
        public string[] HeroImages { get; set; }

        public bool MaintenanceMode { get; set; }

        public DateTime MaintenanceEnd { get; set; }

        public string Announcement { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
