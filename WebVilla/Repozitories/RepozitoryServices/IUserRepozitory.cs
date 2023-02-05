using WebVilla.AuthModels;
using WebVilla.AuthModels.AuthDTOs;

namespace WebVilla.Repozitories.RepozitoryServices
{
    public interface IUserRepozitory
    {
        Task<bool> IsUniqueUser(string userName);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<User> Register(RegistrationRequestDto registrationRequestDto);
    }
}
