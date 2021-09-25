namespace Contactr.DTOs.AuthenticationProvider
{
    public class GoogleLoginDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string IdToken { get; set; }
    }
}