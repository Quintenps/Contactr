using System;
using Contactr.Factories.Interfaces;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.PeopleService.v1;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;

namespace Contactr.Factories
{
    public class PeopleServiceFactory : IPeopleServiceFactory
    {
        public static string GOOGLE_OAUTH_CLIENTID = "GoogleOAuth:clientId";
        public static string GOOGLE_OAUTH_CLIENTSECRET = "GoogleOAuth:clientSecret";
        
        private readonly IConfiguration _configuration;

        public PeopleServiceFactory(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            ConfigCheck();
        }

        private void ConfigCheck()
        {
            var clientId = _configuration.GetValue<string>(GOOGLE_OAUTH_CLIENTID);
            var clientSecret = _configuration.GetValue<string>(GOOGLE_OAUTH_CLIENTSECRET);

            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
                throw new ArgumentNullException(nameof(_configuration), "Google config not found");
        }

        public PeopleServiceService Create(string refreshToken)
        {
            ClientSecrets secrets = new()
            {
                ClientId = _configuration.GetValue<string>(GOOGLE_OAUTH_CLIENTID),
                ClientSecret = _configuration.GetValue<string>(GOOGLE_OAUTH_CLIENTSECRET)
            };

            var token = new TokenResponse { RefreshToken = refreshToken };
            var credentials = new UserCredential(new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = secrets
            }), "user", token);

            return new PeopleServiceService(new BaseClientService.Initializer
            {
                ApplicationName = "Contactr",
                HttpClientInitializer = credentials
            });
        }
    }
}