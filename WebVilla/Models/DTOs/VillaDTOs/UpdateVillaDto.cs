﻿using System.ComponentModel.DataAnnotations;

namespace WebVilla.Models.DTOs.VillaDTOs
{
    public class UpdateVillaDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Details { get; set; }
        public double Rate { get; set; }
        public int Sqft { get; set; }
        public int Occupancy { get; set; }
        public string ImageUrl { get; set; }
        public string Amenity { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
