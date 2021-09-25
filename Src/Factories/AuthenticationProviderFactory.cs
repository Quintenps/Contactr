using System;
using Contactr.Factories.Interfaces;
using Contactr.Models.Authentication;
using Contactr.Models.Enums;

namespace Contactr.Factories
{
    public class AuthenticationProviderFactory : IAuthenticationProviderFactory
    {
        public AuthenticationProvider Create(Guid userId, string key, LoginProviders loginProvider)
        {
            return new AuthenticationProvider()
            {
                UserId = userId,
                Key = key,
                LoginProvider = loginProvider
            };
        }
    }
}