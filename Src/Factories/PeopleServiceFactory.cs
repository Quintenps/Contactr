using System;
using Contactr.Factories.Interfaces;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.PeopleService.v1;
using Google.Apis.PeopleService.v1.Data;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;

namespace Contactr.Factories
{
    public class PeopleServiceFactory : IPeopleServiceFactory
    {
        public static string GOOGLE_OAUTH_CLIENTID = "GoogleOAuth:clientId";
        public static string GOOGLE_OAUTH_CLIENTSECRET = "GoogleOAuth:clientSecret";
        public static string PERSONAL_LABEL = "Personal";
        public static string BUSINESS_LABEL = "Business";
        
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

        public PeopleServiceService CreatePeopleServiceClient(string refreshToken)
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
        
        public Name CreateName(string firstName, string lastName, string fullName)
        {
            return new Name()
            {
                DisplayName = fullName,
                GivenName = firstName,
                FamilyName = lastName,
                UnstructuredName = fullName
            };
        }

        public Organization CreateOrganization(string companyName, string jobTitle)
        {
            return new Organization()
            {
                Name = companyName,
                Title = jobTitle,
                JobDescription = jobTitle,
                FormattedType = jobTitle
            };
        }
        
        public EmailAddress CreateEmail(string type, string emailAddress)
        {
            return new EmailAddress()
            {
                DisplayName = type,
                FormattedType = type,
                Type = type,
                Value = emailAddress
            };
        }

        public Birthday CreateBirthday(Date birthday)
        {
            return new Birthday()
            {
                Date = birthday,
                Text = birthday.ToString()
            };
        }

        public Address CreateAddress(string type, string city, string country, string postalcode, string street)
        {
            return new Address()
            {
                Type = type,
                City = city,
                Country = country,
                PostalCode = postalcode,
                StreetAddress = street
            };
        }

        public UpdateContactPhotoRequest CreatePhoto()
        {
            return new UpdateContactPhotoRequest()
            {
                PhotoBytes = "123"
            };
        }
    }
}