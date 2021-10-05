using System;
using System.Collections.Generic;
using Contactr.Factories.Interfaces;
using Contactr.Models.Connection;
using Contactr.Persistence.Repositories.Interfaces;
using Contactr.Services.DatastoreService;
using Google.Apis.PeopleService.v1;
using Google.Apis.PeopleService.v1.Data;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;

namespace Contactr.Services.GoogleServices
{
    public class SyncService : GoogleService, ISyncService
    {
        private readonly IConnectionRepository _connectionRepository;
        private readonly IDatastoreService _datastoreService;

        private PeopleServiceService _peopleServiceService;
        private Person _person;
        
        public SyncService(ILogger<GoogleService> googleServiceLogger, IDataProtectionProvider dataProtectionProvider, IPeopleServiceFactory peopleServiceFactory, IAuthenticationProviderRepository authenticationProviderRepository, IConnectionRepository connectionRepository, IDatastoreService datastoreService) : base(googleServiceLogger, dataProtectionProvider, peopleServiceFactory, authenticationProviderRepository)
        {
            _connectionRepository = connectionRepository ?? throw new ArgumentNullException(nameof(connectionRepository));
            _datastoreService = datastoreService ?? throw new ArgumentNullException(nameof(datastoreService));
        }

        private void GetPersonByResourceName(IEnumerable<Connection> connections)
        {
            foreach (var connection in connections)
            {
                var googleResource = (Models.Connection.Resources.Google) connection.Resource;
                var getRequest = _peopleServiceService.People.Get(googleResource.ResourceName);
                getRequest.PersonFields = PersonFields;
                _person =  getRequest.Execute();
                
                // _datastoreService.MakeRequest();
            }
        }
        
        public void Synchronize(Guid userId)
        {
            _peopleServiceService = GetPeopleService(userId);
            var connections = _connectionRepository.GetConnections(userId);
            GetPersonByResourceName(connections);

        }
    }
}