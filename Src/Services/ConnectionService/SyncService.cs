using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contactr.Factories.Interfaces;
using Contactr.Models;
using Contactr.Models.Cards;
using Contactr.Models.Connection;
using Contactr.Persistence.Repositories.Interfaces;
using Contactr.Services.ConnectionService.Providers;
using Contactr.Services.DatastoreService;
using Google.Apis.PeopleService.v1;
using Google.Apis.PeopleService.v1.Data;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;

namespace Contactr.Services.ConnectionService
{
    public class SyncService : GoogleService, ISyncService
    {
        private readonly IConnectionRepository _connectionRepository;
        private readonly IDatastoreService _datastoreService;
        private readonly ILogger<SyncService> _logger;
        
        private PeopleServiceService _peopleServiceService;
        private readonly string _updateFields = "names,emailAddresses,addresses,birthdays,genders";

        public SyncService(ILogger<GoogleService> googleServiceLogger, IDataProtectionProvider dataProtectionProvider,
            IPeopleServiceFactory peopleServiceFactory,
            IAuthenticationProviderRepository authenticationProviderRepository,
            IConnectionRepository connectionRepository, IDatastoreService datastoreService, ILogger<SyncService> logger) : base(googleServiceLogger,
            dataProtectionProvider, peopleServiceFactory, authenticationProviderRepository)
        {
            _connectionRepository =
                connectionRepository ?? throw new ArgumentNullException(nameof(connectionRepository));
            _datastoreService = datastoreService ?? throw new ArgumentNullException(nameof(datastoreService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private Dictionary<Guid, Tuple<Person, User>> GetPersonByResourceName(IEnumerable<Connection> connections)
        {
            var dict = new Dictionary<Guid, Tuple<Person, User>>();
            foreach (var connection in connections)
            {
                var googleResource = (Models.Connection.Resources.Google) connection.Resource;
                var getRequest = _peopleServiceService.People.Get(googleResource.ResourceName);
                getRequest.PersonFields = PersonFields;
                var person = getRequest.Execute();
                dict.Add(connection.SenderUserId, new Tuple<Person, User>(person, connection.SenderUser));
            }

            return dict;
        }

        /// <summary>
        /// Updates <see cref="Person"/>'s fields with data from <see cref="PersonalCard"/> and <see cref="BusinessCard"/> 
        /// </summary>
        /// <param name="people"></param>
        /// <returns></returns>
        private List<Person> UpdatePersonFields(Dictionary<Guid, Tuple<Person, User>> people)
        {
            var updatedContacts = new List<Person>();
            foreach (var (_, (person, user)) in people)
            {
                updatedContacts.Add(_datastoreService.UpdateFields(user.PersonalCard, user.BusinessCards, person));
            }

            return updatedContacts;
        }

        private async Task ExecuteUpdatedPersons(IEnumerable<Person> updatedPersons)
        {
            var pagesize = 200; // Maximum BatchUpdateContactsRequest update size
            var enumerable = updatedPersons.ToList();
            var pages = Math.Ceiling(enumerable.Count() / 200.0);
            for (int currentPage = 0; currentPage < pages; currentPage++)
            {
                var persons = enumerable.Take(pagesize).Skip(currentPage * pagesize)
                    .ToDictionary(p => p.ResourceName, p => p);
                var batchUpdateRequest = new BatchUpdateContactsRequest
                {
                    Contacts = persons,
                    UpdateMask = _updateFields
                };
                try
                {
                    await _peopleServiceService.People.BatchUpdateContacts(batchUpdateRequest).ExecuteAsync();
                }
                catch (Exception e)
                {
                    _logger.LogInformation("An error occured during ExecuteUpdatedPersons");
                    _logger.LogError(e.Message);
                }
            }
        }

        public async Task Synchronize(Guid userId)
        {
            _peopleServiceService = GetPeopleService(userId);
            var connections = _connectionRepository.GetConnections(userId);
            var people = GetPersonByResourceName(connections);
            var updatedPeople = UpdatePersonFields(people);
            await ExecuteUpdatedPersons(updatedPeople);
        }
    }
}