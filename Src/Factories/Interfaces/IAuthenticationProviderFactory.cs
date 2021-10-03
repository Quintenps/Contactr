using System;
using Contactr.Models.Authentication;
using Contactr.Models.Enums;

namespace Contactr.Factories.Interfaces
{
    public interface IAuthenticationProviderFactory
    {
        public AuthenticationProvider Create(Guid userId, string key, LoginProviders loginProvider, string refreshToken);
    }
}