namespace Contactr.DTOs.AuthenticationProvider
{
    public class GoogleLoginDto
    {
        public string RefreshToken { get; set; } = null!;
        public string IdToken { get; set; } = null!;
    }
}