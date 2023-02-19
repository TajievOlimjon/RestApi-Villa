namespace WebVilla.AuthModels.AuthDTOs
{
    public class LoginResponseDto
    {
        public UserDto User { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}
