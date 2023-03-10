using System.ComponentModel.DataAnnotations;

namespace Web.Models.DTOs.VillaNumberDtos
{
    public class CreateVillaNumberDto
    {
        [Required]
        public int VillaNo { get; set; }
        [Required]
        public int VillaId { get; set; }
        public string SpecialDetails { get; set; }
    }
}
