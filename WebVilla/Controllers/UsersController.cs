using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebVilla.AuthModels.AuthDTOs;
using WebVilla.Repozitories.RepozitoryServices;
using WebVilla.Responses;

namespace WebVilla.Controllers
{
    [Route("api/v{version:apiVersion}[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepozitory _userRepozitory;
        private APIResponse _apiResponse;
        public UsersController(IUserRepozitory userRepozitory)
        {
            _userRepozitory = userRepozitory;
            _apiResponse = new();
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var response = await _userRepozitory.Login(loginRequestDto);
            if (response.User is null || string.IsNullOrEmpty(response.Token))
            {
                _apiResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string> { "UserName or password  is incorrect !" };
                return BadRequest(_apiResponse);
            }
            _apiResponse.StatusCode = (int)HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            _apiResponse.Result = response;
            return Ok(_apiResponse);

        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            var response = await _userRepozitory.IsUniqueUser(registrationRequestDto.UserName);
            if (response is false)
            {
                _apiResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string> { "Not success !" };
                return BadRequest(_apiResponse);
            }
            var registerResponse = await _userRepozitory.Register(registrationRequestDto);
            if (registerResponse is null)
            {
                _apiResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string> { "Not success !" };
                return BadRequest(_apiResponse);
            }
            _apiResponse.StatusCode = (int)HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }
    }
}
