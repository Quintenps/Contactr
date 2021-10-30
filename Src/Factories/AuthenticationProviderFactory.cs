using System;
using Contactr.Factories.Interfaces;
using Contactr.Models.Authentication;
using Contactr.Models.Enums;
using Microsoft.AspNetCore.DataProtection;

namespace Contactr.Factories
{
    public class AuthenticationProviderFactory : IAuthenticationProviderFactory
    {
        private readonly IDataProtector _dataProtector;

        public AuthenticationProviderFactory(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtector = dataProtectionProvider.CreateProtector(DataProtectionKeys.AuthProviderRefreshToken.ToString()) ?? throw new ArgumentNullException(nameof(dataProtectionProvider));
        }

        public AuthenticationProvider Create(Guid userId, LoginProviders loginProvider, string refreshToken)
        {
            return new AuthenticationProvider()
            {
                UserId = userId,
                LoginProvider = loginProvider,
                RefreshToken = _dataProtector.Protect(refreshToken)
            };
        }
    }
}