using LocationTracking.Grains;
using System.ComponentModel.DataAnnotations;

namespace LocationTracking.Api.Models
{
    public class LocationModel
    {
        public Location ToLocation()
            => new Location { DateTime = DateTime, Latitude = Latitude, Longitude = Longitude };

        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
    }
}
