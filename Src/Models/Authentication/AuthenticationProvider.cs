using System;
using Contactr.Models.Enums;

namespace Contactr.Models.Authentication
{
    public class AuthenticationProvider
    {
        public LoginProviders LoginProvider { get; set; }
        public string Key { get; set; } = null!;
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}