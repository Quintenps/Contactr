namespace Contactr.DTOs.AuthenticationProvider
{
    public class GoogleLoginResponse
    {
        public string Sub { get; set; }
        public string Picture { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
    }
}