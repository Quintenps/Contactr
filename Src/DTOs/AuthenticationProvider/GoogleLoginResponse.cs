namespace Contactr.DTOs.AuthenticationProvider
{
    public class GoogleLoginResponse
    {
        public string Sub { get; set; } = null!;
        public string Picture { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string GivenName { get; set; } = null!;
        public string FamilyName { get; set; } = null!;
    }
}