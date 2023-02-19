using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebVilla.AuthModels;
using WebVilla.AuthModels.AuthDTOs;
using WebVilla.Data;

namespace WebVilla.Repozitories.RepozitoryServices
{
    public class UserRepozitory : IUserRepozitory
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private string secretKey;
        public UserRepozitory(ApplicationContext context,IConfiguration configuration,
            UserManager<ApplicationUser> userManager,IMapper mapper)
        {
            _userManager = userManager;
            _context = context;
            secretKey = configuration.GetSection("SecretKey:Key").Value;
            _mapper = mapper;
        }
        public async Task<bool> IsUniqueUser(string userName)
        {
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(x=>x.UserName.ToLower().Equals(userName.ToLower()));
            if(user is null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user= await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.UserName.ToLower().Equals(loginRequestDto.UserName.ToLower()));
            var resultCheckPassword = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            if(user is null || resultCheckPassword is false)
            {
                return new LoginResponseDto
                {
                    User = null,
                    Token = "Not token"
                };
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject=new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(ClaimTypes.Role,userRoles.First())
                }),
                Expires=DateTime.UtcNow.AddDays(1),
                SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
            };

            var createToken = jwtTokenHandler.CreateToken(tokenDescriptor);
            var writeToken = jwtTokenHandler.WriteToken(createToken);

            var response = new LoginResponseDto
            {
                User=_mapper.Map<UserDto>(user),
                Role = userRoles.First(),
                Token = writeToken
            };
            return response;
        }

        public async Task<RegistrationResponseDto> Register(RegistrationRequestDto registrationRequestDto)
        {
            var user = new ApplicationUser
            {
                FirstName = registrationRequestDto.FirstName,
                LastName = registrationRequestDto.LastName,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                UserName = registrationRequestDto.UserName,
                NormalizedUserName = registrationRequestDto.UserName.ToUpper()
            };

           var result= await _userManager.CreateAsync(user,registrationRequestDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user,"Admin");

                var response = new RegistrationResponseDto
                {
                    FullName = string.Concat(user.FirstName + " " + user.LastName),
                    UserName = user.UserName,
                    Email = user.Email
                };
                return response;
            }
            return null;
        }
    }
}
