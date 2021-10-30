using System;
using Contactr.Models.Enums;

namespace Contactr.Models.Authentication
{
    public class AuthenticationProvider
    {
        public Guid Id { get; set; }
        public LoginProviders LoginProvider { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}