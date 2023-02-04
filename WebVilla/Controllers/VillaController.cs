﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebVilla.Models;
using WebVilla.Models.DTOs.VillaDTOs;
using WebVilla.Repozitories.RepozitoryServices;
using WebVilla.Responses;

namespace WebVilla.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        protected APIResponse _response;
        private  readonly IVillaRepozitory  _villaRepozitory;
        private readonly IMapper _mapper;
        public VillaController(IVillaRepozitory  villaRepozitory,IMapper mapper)
        {
            _villaRepozitory=villaRepozitory;
            _mapper = mapper;
            this._response = new();
        }

        [HttpGet("GetVillas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            { 
                var villas = await _villaRepozitory.GetAllAsync();
                
                if (villas.Count is  0)
                {
                    _response.Result= new List<GetAllVillaDto>();
                    _response.StatusCode = (int)HttpStatusCode.NoContent;

                    return BadRequest(_response);
                }
                
                _response.Result = _mapper.Map<List<GetAllVillaDto>>(villas);
                _response.StatusCode = (int)HttpStatusCode.OK;
                /*_response.Result= villas.Select(x=>new GetAllVillaDto
                {
                    Id=x.Id,
                    Name=x.Name,
                    Details=x.Details,
                    Sqft=x.Sqft,
                    Amenity=x.Amenity,
                    ImageUrl=x.ImageUrl,
                    Occupancy=x.Occupancy,
                    Rate=x.Rate,
                    CreatedAt=x.CreatedAt,
                    UpdatedAt=x.UpdatedAt
                }).ToList();*/
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(statusCode: 400)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
               
                 return  CreatedAtRoute("GetVillaById", new { Id=villa.Id },_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }
  
        [HttpGet("{id:int}",Name ="GetVillaById")]
        /*[Route("GetVillaById")]*/
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try 
            {
                 if(id is 0)
                 {
                    _response.StatusCode = (int)HttpStatusCode.BadRequest;
                     return BadRequest(_response);
                 }
                 

                 var item=await _villaRepozitory.GetAsync(i=>i.Id==id);

                 if(item is null)
                 {
                     ModelState.AddModelError(string.Empty,$"Not found!, no data in server owned by id:{id}");
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVilla(UpdateVillaDto updateVillaDto)
        {
            try
            { 
                 if(updateVillaDto.Id is 0)
                 {
                    _response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                 }
                 var villa = await _villaRepozitory.GetAsync(x => x.Id.Equals(updateVillaDto.Id));
                if(villa is null)
                {
                    _response.StatusCode = (int)HttpStatusCode.NotFound;
                    return BadRequest(_response);
                }
                 var model = _mapper.Map<Villa>(updateVillaDto);
                 model.CreatedAt = villa.CreatedAt;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() {ex.ToString()};
            }
            return _response;
        }
    }
}
