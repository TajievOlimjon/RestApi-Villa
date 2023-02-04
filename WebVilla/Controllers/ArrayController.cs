using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebVilla.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArrayController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            int[] array = { 1, 2, 5, 2, 1, 6, 5 };
            //var query = array.GroupBy(z => z).Where(x => x.Count() > 1).ToList();

            var qroupBy = array.GroupBy(x=>x);

            var whereArr = qroupBy.Where(x => x.Count() > 1);

            var list = whereArr.ToList();

           
                 
            return Ok(list);
        }
    }
}
