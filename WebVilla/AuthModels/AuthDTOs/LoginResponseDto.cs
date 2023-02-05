namespace WebVilla.AuthModels.AuthDTOs
{
    public class LoginResponseDto
    {
        public User User { get; set; }
        public string Token { get; set; }
    }
}
