using System;
using Contactr.Models.Enums;

namespace Contactr.Models.Authentication
{
    public class AuthenticationProvider
    {
        public LoginProviders LoginProvider { get; set; }
        public string Key { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}