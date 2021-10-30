using System;
using Contactr.Models.Authentication;
using Contactr.Models.Enums;

namespace Contactr.Factories.Interfaces
{
    public interface IAuthenticationProviderFactory
    {
        public AuthenticationProvider Create(Guid userId, LoginProviders loginProvider, string refreshToken);
    }
}