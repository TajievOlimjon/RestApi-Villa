using System.ComponentModel.DataAnnotations;

namespace Web.Models.DTOs.VillaNumberDtos
{
    public class GetAllVillaNumberDto
    {
        public int VillaNo { get; set; }
        public int VillaId { get; set; }
        public string SpecialDetails { get; set; }
        public DateTimeOffset CreatedAt { get; set; } 
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
