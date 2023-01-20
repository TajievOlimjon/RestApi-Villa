using System.ComponentModel.DataAnnotations;

namespace WebVilla.Models.DTOs
{
    public class CreateVillaDto
    {
        [Required]
        public string Name { get; set; }
        public string Details { get; set; }
        public double Rate { get; set; }
        public int Sqft { get; set; }
        public int Occupancy { get; set; }
        public IFormFile ImageUrl { get; set; }
        public string Amenity { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
