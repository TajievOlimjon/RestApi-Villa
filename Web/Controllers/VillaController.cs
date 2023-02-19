using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Web.Models.DTOs.VillaDTOs;
using Web.Responses;
using Web.Services.IServices;

namespace Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;
        public VillaController(IVillaService villaService, IMapper mapper)
        {
            _villaService = villaService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            List<GetAllVillaDto> villas = null;
            var response = await _villaService.GetAllAsync<APIResponse>();
            if(response is not null && response.IsSuccess)
            {
                villas = JsonConvert.DeserializeObject<List<GetAllVillaDto>>(Convert.ToString(response.Result));
            }
            return View(villas);
        }
    }
}
