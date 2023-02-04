using System.ComponentModel.DataAnnotations;

namespace WebVilla.Models.DTOs.VillaNumberDtos
{
    public class GetVillaNumberDto
    {
        public int VillaNo { get; set; }
        public int VillaId { get; set; }
        public string SpecialDetails { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
