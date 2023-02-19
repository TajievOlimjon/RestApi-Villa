using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using WebVilla.Models;
using WebVilla.Models.DTOs.VillaDTOs;
using WebVilla.PaginationModels;
using WebVilla.Repozitories.RepozitoryServices;
using WebVilla.Responses;

namespace WebVilla.Controllers.v1
{
    [Route("api/v{version:apiVersion}[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class VillaController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IVillaRepozitory _villaRepozitory;
        private readonly IMapper _mapper;
        public VillaController(IVillaRepozitory villaRepozitory, IMapper mapper)
        {
            _villaRepozitory = villaRepozitory;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet("GetVillas")]
        [MapToApiVersion("1.0")]
        //[Authorize]
        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<APIResponse>> GetVillas([FromQuery(Name ="FilterOccupancy")]int? occupancy, 
            [FromQuery(Name ="Search by name and amenity")]string? search, int pageSize = 0, int pageNumber = 1)
        {
            try
            {
                List<Villa> getAllVillas = new();
                if (occupancy > 0)
                {
                    getAllVillas = await _villaRepozitory.GetAllAsync(x=>x.Occupancy==occupancy,
                                                          pageSize:pageSize,pageNumber:pageNumber);
                }
                else
                {
                    getAllVillas = await _villaRepozitory.GetAllAsync(pageSize: pageSize, pageNumber: pageNumber);
                }
                if (!string.IsNullOrEmpty(search))
                {
                    getAllVillas = getAllVillas.Where(x => x.Name.ToLower()
                        .Contains(search.ToLower().Trim()) || x.Amenity.ToLower()
                        .Contains(search.ToLower().Trim())).ToList();
                }
                Pagination pagination = new Pagination
                {
                    PageSize=pageSize,
                    PageNumber=pageNumber
                };
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
                if (getAllVillas.Count is 0)
                {
                    _response.Result = new List<GetAllVillaDto>();
                    _response.StatusCode = (int)HttpStatusCode.NoContent;

                    return BadRequest(_response);
                }

                _response.Result = _mapper.Map<List<GetAllVillaDto>>(getAllVillas);
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
        [HttpPost("AddVilla")]
        [ResponseCache(Duration = 30)]
        // [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<APIResponse>> AddVilla([FromBody] CreateVillaDto villaDto)
        {
            try
            {
                if (ModelState.IsValid is false)
                {
                    ModelState.AddModelError(string.Empty, "error !");
                    return BadRequest(ModelState);
                }

                // var result=await
                if (await _villaRepozitory.GetAsync(x => x.Name.ToLower().Equals(villaDto.Name.ToLower())) is not null)
                {
                    ModelState.AddModelError("CustomError", $"This is {villaDto.Name} villa already exists!");

                    return BadRequest(ModelState);
                }

                var villa = _mapper.Map<Villa>(villaDto);
                villa.CreatedAt = DateTime.UtcNow;
                await _villaRepozitory.CreateAsync(villa);

                _response.Result = _mapper.Map<CreateVillaDto>(villa);
                _response.StatusCode = (int)HttpStatusCode.Created;

                return CreatedAtRoute("GetVillaById", new { id = villa.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("{id:int}", Name = "GetVillaById")]
        /*[Route("GetVillaById")]*/
      //  [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id is 0)
                {
                    _response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }


                var item = await _villaRepozitory.GetAsync(i => i.Id == id);

                if (item is null)
                {
                    ModelState.AddModelError(string.Empty, $"Not found!, no data in server owned by id:{id}");
                    return NotFound(ModelState);
                }
                _response.Result = _mapper.Map<GetVillaDto>(item);
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
        [HttpPut("UpdateVilla")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<APIResponse>> UpdateVilla(UpdateVillaDto updateVillaDto)
        {
            try
            {
                if (updateVillaDto.Id is 0)
                {
                    _response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var model = _mapper.Map<Villa>(updateVillaDto);
                await _villaRepozitory.UpdateAsync(model);
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

        [HttpDelete("DeleteVilla")]
        [Authorize(Roles = "Custom")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {
            try
            {
                if (id is 0)
                {
                    _response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var villa = await _villaRepozitory.GetAsync(x => x.Id.Equals(id));

                if (villa is null)
                {
                    ModelState.AddModelError(string.Empty, $"Not found!, no data in server owned by id:{id}");
                    return NotFound(ModelState);
                }
                await _villaRepozitory.RemoveAsync(villa);
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
    }
}
