using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebVilla.Models;
using WebVilla.Models.DTOs.VillaDTOs;
using WebVilla.Models.DTOs.VillaNumberDtos;
using WebVilla.Repozitories.RepozitoryServices;
using WebVilla.Responses;

namespace WebVilla.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaNumberController : ControllerBase
    {
        protected APIResponse _response;
        private  readonly IVillaNumberRepozitory  _villaNumberRepozitory;
        private readonly IVillaRepozitory _villaRepozitory;
        private readonly IMapper _mapper;
        public VillaNumberController(IVillaNumberRepozitory villaNumberRepozitory,IVillaRepozitory villaRepozitory,IMapper mapper)
        {
            _villaNumberRepozitory = villaNumberRepozitory;
            _villaRepozitory = villaRepozitory;
            _mapper = mapper;
            this._response = new();
        }

        [HttpGet("GetVillaNumbers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetVillaNumbers()
        {
            try
            { 
                var villas = await _villaNumberRepozitory.GetAllAsync();
                
                if (villas.Count is  0)
                {
                    _response.Result= _mapper.Map<List<GetAllVillaNumberDto>>(new List<VillaNumber>());
                    return Ok(_response);
                }
                _response.Result= _mapper.Map<List<GetAllVillaNumberDto>>(villas);
                _response.StatusCode =(int)HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [HttpPost("AddVillaNumber")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(statusCode: 400)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> AddVillaNumber([FromBody] CreateVillaNumberDto villaNumberDto)
        {
            try
            { 
                 if (ModelState.IsValid is false)
                 {
                     ModelState.AddModelError(string.Empty, "error !");
                     return BadRequest(ModelState);
                 }

                // var result=await
                 if (await _villaNumberRepozitory.GetAsync(x => x.VillaNo.Equals(villaNumberDto.VillaNo)) is not null)
                 {
                     ModelState.AddModelError("CustomError", $"This is {villaNumberDto.VillaNo} villa number already exists!");
               
                     return BadRequest(ModelState);
                 }

                 if(await _villaRepozitory.GetAsync(x=>x.Id.Equals(villaNumberDto.VillaId)) is null)
                 {
                    ModelState.AddModelError("CustomError","villa id is invalid");

                    return BadRequest(ModelState);
                 }
               
                 var villaNumber = _mapper.Map<VillaNumber>(villaNumberDto);
                 villaNumber.CreatedAt = DateTime.UtcNow;
                 await _villaNumberRepozitory.CreateAsync(villaNumber);
               
                 _response.Result = _mapper.Map<CreateVillaNumberDto>(villaNumber);
                 _response.StatusCode = (int)HttpStatusCode.Created;
               
                 return  CreatedAtRoute("GetVillaNumberById", new { villaNo= villaNumber.VillaNo},_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }
  
        [HttpGet("{villaNo:int}", Name ="GetVillaNumberById")]
        /*[Route("GetVillaById")]*/
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVillaNumberById(int villaNo)
        {
            try 
            {
                 if(villaNo is 0)
                 {
                    _response.StatusCode = (int)HttpStatusCode.BadRequest;
                     return BadRequest(_response);
                 }
                 

                 var item=await _villaNumberRepozitory.GetAsync(i=>i.VillaNo== villaNo);

                 if(item is null)
                 {
                     ModelState.AddModelError(string.Empty,$"Not found!, no data in server owned by nomer:{villaNo}");
                     return NotFound(ModelState);
                 }
                 _response.Result = _mapper.Map<GetVillaNumberDto>(item);
                 _response.StatusCode = (int)HttpStatusCode.OK;
                 return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [HttpPut("UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVillaNumber(UpdateVillaNumberDto updateVillaNumberDto)
        {
            try
            { 
                 if(updateVillaNumberDto.VillaNo is 0)
                 {
                    _response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                 }
                var model = await _villaRepozitory.GetAsync(x => x.Id.Equals(updateVillaNumberDto.VillaId));
                if (model is null)
                {
                    ModelState.AddModelError("CustomError", "villa id is invalid");

                    return BadRequest(ModelState);
                }
                 var villaNumber = _mapper.Map<VillaNumber>(updateVillaNumberDto);
                 villaNumber.CreatedAt = model.CreatedAt;
                 await _villaNumberRepozitory.UpdateAsync(villaNumber);
                 _response.StatusCode = (int)HttpStatusCode.OK;
                 _response.IsSuccess = true;
                 return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;

        }

        [HttpDelete("DeleteVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int villaNo)
        {
            try
            {
                if (villaNo is 0)
                {
                    _response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var villa = await _villaNumberRepozitory.GetAsync(x => x.VillaNo.Equals(villaNo));

                if (villa is null)
                {
                    ModelState.AddModelError(string.Empty, $"Not found!, no data in server owned by nomer:{villaNo}");
                    return NotFound(ModelState);
                }
                await _villaNumberRepozitory.RemoveAsync(villa);
                _response.StatusCode = (int)HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() {ex.ToString()};
            }
            return _response;
        }
    }
}
