using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebVilla.AuthModels;
using WebVilla.AuthModels.AuthDTOs;
using WebVilla.Data;
using WebVilla.Repozitories.Repozitory;

namespace WebVilla.Repozitories.RepozitoryServices
{
    public class UserRepozitory : IUserRepozitory
    {
        private readonly ApplicationContext _context;
        private string secretKey;
        public UserRepozitory(ApplicationContext context,IConfiguration configuration)
        {
            _context = context;
            secretKey = configuration.GetSection("SecretKey:Key").Value;
        }
        public async Task<bool> IsUniqueUser(string userName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x=>x.UserName.ToLower().Equals(userName.ToLower()));
            if(user is null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user= await _context.Users.FirstOrDefaultAsync(x => x.UserName.ToLower().Equals(loginRequestDto.UserName.ToLower()));
            if(user is null)
            {
                return new LoginResponseDto
                {
                    User = null,
                    Token = "Not token"
                };
            }
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject=new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(ClaimTypes.Role,user.Role)
                }),
                Expires=DateTime.UtcNow.AddDays(1),
                SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
            };

            var createToken = jwtTokenHandler.CreateToken(tokenDescriptor);
            var writeToken = jwtTokenHandler.WriteToken(createToken);

            var response = new LoginResponseDto
            {
                User=user,
                Token = writeToken
            };
            return response;
        }

        public async Task<User> Register(RegistrationRequestDto registrationRequestDto)
        {
            var user = new User
            {
                UserName = registrationRequestDto.UserName,
                Name = registrationRequestDto.Name,
                Password = registrationRequestDto.Password,
                Role = registrationRequestDto.Role
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            user.Password = "Я думаю вам не нужно видеть пароль";

            return user;
        }
    }
}
