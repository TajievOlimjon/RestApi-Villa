using System.ComponentModel.DataAnnotations;

namespace WebVilla.Models.DTOs.VillaNumberDtos
{
    public class UpdateVillaNumberDto
    {
        [Required]
        public int VillaNo { get; set; }
        [Required]
        public int VillaId { get; set; }
        public string SpecialDetails { get; set; }
    }
}
