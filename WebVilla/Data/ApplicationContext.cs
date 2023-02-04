﻿using Microsoft.EntityFrameworkCore;
using WebVilla.Models;

namespace WebVilla.Data
{
    public class ApplicationContext:DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {

        }
        public DbSet<Villa> Villas { get; set; }
        public DbSet<VillaNumber> VillaNumbers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id = 1,
                    Name="Royal Villa",
                    Details= "",
                    ImageUrl= "/Images/Villa.jpeg",
                    Occupancy =1,
                    Rate=300,
                    Sqft=550,
                    Amenity="",
                    CreatedAt=DateTime.UtcNow
                },
                new Villa()
                 {
                     Id = 2,
                     Name = "Luxury Villa",
                     Details = "",
                     ImageUrl = "/Images/Villa3.jpeg",
                     Occupancy = 1,
                     Rate = 300,
                     Sqft = 550,
                     Amenity = "",
                     CreatedAt = DateTime.UtcNow
                 }
             );
            base.OnModelCreating(builder);
        }
    }
}
