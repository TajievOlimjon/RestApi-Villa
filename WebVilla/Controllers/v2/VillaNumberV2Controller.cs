using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebVilla.Models;
using WebVilla.Models.DTOs.VillaNumberDtos;
using WebVilla.Repozitories.RepozitoryServices;
using WebVilla.Responses;

namespace WebVilla.Controllers.v2
{
    [Route("api/v{version:apiVersion}[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
    public class VillaNumberV2Controller : ControllerBase
    {
        protected APIResponse _response;
        private readonly IVillaNumberRepozitory _villaNumberRepozitory;
        private readonly IVillaRepozitory _villaRepozitory;
        private readonly IMapper _mapper;
        public VillaNumberV2Controller(IVillaNumberRepozitory villaNumberRepozitory, IVillaRepozitory villaRepozitory, IMapper mapper)
        {
            _villaNumberRepozitory = villaNumberRepozitory;
            _villaRepozitory = villaRepozitory;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet("GetVillaNumbersv2")]
        [MapToApiVersion("2.0")]
        public IEnumerable<string> Get()
        {
            return new string[] { ".Net", "practics" };
        }


    }
}
