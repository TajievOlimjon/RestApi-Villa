using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebVilla.Migrations
{
    public partial class AddSeedDataVilla : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenity", "CreatedAt", "Details", "ImageUrl", "Name", "Occupancy", "Rate", "Sqft", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "", new DateTime(2023, 1, 20, 13, 29, 14, 848, DateTimeKind.Utc).AddTicks(8227), "", "/Images/Villa.jpeg", "Royal Villa", 1, 300.0, 550, new DateTime(2023, 1, 20, 13, 29, 14, 848, DateTimeKind.Utc).AddTicks(8224) },
                    { 2, "", new DateTime(2023, 1, 20, 13, 29, 14, 848, DateTimeKind.Utc).AddTicks(8232), "", "/Images/Villa3.jpeg", "Luxury Villa", 1, 300.0, 550, new DateTime(2023, 1, 20, 13, 29, 14, 848, DateTimeKind.Utc).AddTicks(8230) }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
