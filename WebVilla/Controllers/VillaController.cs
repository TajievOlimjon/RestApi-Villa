using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebVilla.Data;
using WebVilla.Logging;
using WebVilla.Models;

namespace WebVilla.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        public  readonly ApplicationContext _context;
        private readonly ILogging _logger;
        public VillaController(ILogging logger,ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("GetVillas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: 400)]
        [ProducesResponseType(statusCode: 404)]
        public IActionResult GetVillas()
        {
            var villas = _context.Villas.ToList();
            _logger.Log("Getting all Villas", "information");
            if(villas.Count is not 0) return Ok(villas);
            ModelState.AddModelError("CustomError", "Not content");
            return BadRequest(ModelState);
        }
        [HttpPost("AddVilla")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(statusCode: 400)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddVilla([FromBody]Villa villa)
        {
            int cnt = 0;
            if (ModelState.IsValid is false)
            {
                ModelState.AddModelError(string.Empty, "error !");
                return BadRequest(ModelState);
            }


            if (_context.Villas.FirstOrDefault(x => x.Name.ToLower() == villa.Name.ToLower())!= null)
            {
                ModelState.AddModelError("CustomError",$"This is {villa.Name} villa already exists!");

                return BadRequest(ModelState);
            }
            if (villa is not null)
            {

                _context.Villas.Add(villa);
                var x = _context.SaveChanges();
                if (x is 0)
                {
                    ModelState.AddModelError("No success","No created");
                    return BadRequest();
                    
                }
                else
                { 
                  //return Ok("success");
                  return  CreatedAtRoute("GetVillaById", new { Id=villa.Id },villa);
                    
                }
            }
            return BadRequest();
          
        }
  
        [HttpGet("{id:int}",Name ="GetVillaById")]
        /*[Route("GetVillaById")]*/
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetVilla(int id)
        {
            if(id is 0)
            {
                _logger.Log($"Error, id cannot be zero:{id}","error");
                return BadRequest();
            }
            

            var item= _context.Villas.FirstOrDefault(i=>i.Id==id);

            if(item is null)
            {
                ModelState.AddModelError(string.Empty,$"Not found!,no data on base owned by id:{id}");
                return NotFound(ModelState);
            }
            return Ok(item);
        }
        [HttpPatch("{id:int}",Name = "UpdatePartialVilla")]
       /* [Route("PartialVilla")]*/
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialVilla(int id,JsonPatchDocument<Villa> jsonPatch)
        {
            if(id is 0 || jsonPatch is null)
            {
                return BadRequest();
            }
            var villa = _context.Villas.FirstOrDefault(x => x.Id == id);
            if(villa is null)
            {
                return BadRequest();
            }
            jsonPatch.ApplyTo(villa,ModelState);
            if(ModelState.IsValid is false)
            {
                return BadRequest();
            }
            return NoContent();
        }
    }
}
