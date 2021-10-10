using System;
using Contactr.Factories.Interfaces;
using Contactr.Models.Authentication;
using Contactr.Models.Enums;
using Contactr.Persistence.Repositories.Interfaces;
using Google.Apis.PeopleService.v1;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;
using static Google.Apis.PeopleService.v1.PeopleResource.ConnectionsResource;

namespace Contactr.Services.GoogleServices
{
    public abstract class GoogleService : IGoogleService
    {
        private readonly ILogger<GoogleService> _logger;
        private readonly IDataProtector _dataProtector;
        private readonly IPeopleServiceFactory _peopleServiceFactory;
        private readonly IAuthenticationProviderRepository _authenticationProviderRepository;

        protected const string PersonFields = "names,emailAddresses,phoneNumbers";

        protected GoogleService(ILogger<GoogleService> googleServiceLogger, IDataProtectionProvider dataProtectionProvider, IPeopleServiceFactory peopleServiceFactory, IAuthenticationProviderRepository authenticationProviderRepository)
        {
            _logger = googleServiceLogger ?? throw new ArgumentNullException(nameof(googleServiceLogger));
            _dataProtector = _dataProtector = dataProtectionProvider.CreateProtector(DataProtectionKeys.AuthProviderRefreshToken.ToString()) ?? throw new ArgumentNullException(nameof(dataProtectionProvider));
            _peopleServiceFactory = peopleServiceFactory ?? throw new ArgumentNullException(nameof(peopleServiceFactory));
            _authenticationProviderRepository = authenticationProviderRepository ?? throw new ArgumentNullException(nameof(authenticationProviderRepository));
        }

        public AuthenticationProvider GetAuthenticationProvider(Guid userId)
        {
            var authenticationProvider = _authenticationProviderRepository.SingleOrDefault(ap => ap.UserId.Equals(userId));
            if (authenticationProvider is null)
            {
                _logger.LogError($"Authentication provider not found for userId {userId}");
                throw new ArgumentException("Authenticationprovider not found");
            }

            return authenticationProvider;
        }

        public PeopleServiceService GetPeopleService(Guid userId)
        {
            var authenticationProvider = GetAuthenticationProvider(userId);
            return _peopleServiceFactory.CreatePeopleServiceClient(_dataProtector.Unprotect(authenticationProvider.RefreshToken));
        }

        public ListRequest GetListRequest(Guid userId)
        {
            var service = GetPeopleService(userId);
            ListRequest peopleRequest = service.People.Connections.List("people/me");
            peopleRequest.PersonFields = PersonFields;

            return peopleRequest;
        }
    }
}