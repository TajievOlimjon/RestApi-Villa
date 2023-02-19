using AutoMapper;
using WebVilla.AuthModels;
using WebVilla.AuthModels.AuthDTOs;
using WebVilla.Models;
using WebVilla.Models.DTOs.VillaDTOs;
using WebVilla.Models.DTOs.VillaNumberDtos;

namespace WebVilla.MapModels.VillaMappers
{
    public class MapperDto:Profile
    {
        public MapperDto()
        {
            CreateMap<CreateVillaDto,Villa>().ReverseMap();
            CreateMap<UpdateVillaDto, Villa>().ReverseMap();
            CreateMap<Villa, GetAllVillaDto>().ReverseMap();
            CreateMap<Villa,GetVillaDto>().ReverseMap();


            CreateMap<CreateVillaNumberDto, VillaNumber>().ReverseMap();
            CreateMap<UpdateVillaNumberDto, VillaNumber>().ReverseMap();
            CreateMap<VillaNumber, GetAllVillaNumberDto>().ReverseMap();
            CreateMap<VillaNumber, GetVillaNumberDto>().ReverseMap();

            CreateMap<ApplicationUser, UserDto>().ReverseMap();
        }
    }
}
