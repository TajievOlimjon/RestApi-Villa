using AutoMapper;
using Web.Models;
using Web.Models.DTOs.VillaDTOs;
using Web.Models.DTOs.VillaNumberDtos;
namespace Web.MapModels.VillaMappers
{
    public class MapperDto:Profile
    {
        public MapperDto()
        {
            CreateMap<CreateVillaDto, Models.Villa>().ReverseMap();
            CreateMap<UpdateVillaDto, Models.Villa>().ReverseMap();
            CreateMap<Models.Villa, GetAllVillaDto>().ReverseMap();
            CreateMap<Models.Villa, GetVillaDto>().ReverseMap();


            CreateMap<CreateVillaNumberDto, VillaNumber>().ReverseMap();
            CreateMap<UpdateVillaNumberDto, VillaNumber>().ReverseMap();
            CreateMap<VillaNumber, GetAllVillaNumberDto>().ReverseMap();
            CreateMap<VillaNumber, GetVillaNumberDto>().ReverseMap();
        }
    }
}
