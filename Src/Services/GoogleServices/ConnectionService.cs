using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contactr.Factories.Interfaces;
using Contactr.Models.Cards;
using Contactr.Models.Connection;
using Contactr.Models.Connection.Resources;
using Contactr.Persistence;
using Contactr.Persistence.Repositories.Interfaces;
using Google.Apis.PeopleService.v1;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;

namespace Contactr.Services.GoogleServices
{
    /// <summary>
    /// Service that is used for reading user's Google contacts and creating <see cref="Connection"/>'s
    /// </summary>
    public class ConnectionService : GoogleService, IConnectionService
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IPersonalCardRepository _personalCardRepository;
        private readonly IConnectionRepository _connectionRepository;
        private readonly ILogger<ConnectionService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public ConnectionService(ILogger<GoogleService> googleServiceLogger, IDataProtectionProvider dataProtectionProvider, IPeopleServiceFactory peopleServiceFactory, IAuthenticationProviderRepository authenticationProviderRepository,
            IConnectionFactory connectionFactory, IPersonalCardRepository personalCardRepository, IConnectionRepository connectionRepository, ILogger<ConnectionService> connectionServiceLogger, IUnitOfWork unitOfWork) : base(googleServiceLogger, dataProtectionProvider,
            peopleServiceFactory, authenticationProviderRepository)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _personalCardRepository = personalCardRepository ?? throw new ArgumentNullException(nameof(personalCardRepository));
            _connectionRepository = connectionRepository ?? throw new ArgumentNullException(nameof(connectionRepository));
            _logger = connectionServiceLogger ?? throw new ArgumentNullException(nameof(connectionServiceLogger));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        /// <summary>
        /// Creates <see cref="Connection" /> and saves it to the database
        /// </summary>
        /// <param name="matches"></param>
        /// <param name="userId"></param>
        private async Task CreateConnection(Dictionary<Guid, Resource> matches, Guid userId)
        {
            _logger.LogInformation($"Creating {matches.Count} matches for userId {userId}");
            foreach (var pair in matches)
            {
                _logger.LogInformation($"Creating connection from userId {pair.Key} to {userId}");
                var resource = (Models.Connection.Resources.Google) pair.Value;
                var connection = _connectionFactory.Create(userId, pair.Key, resource.ETag, resource.ResourceName);
                _unitOfWork.ConnectionRepository.Add(connection);
            }
            await _unitOfWork.Save();
        }

        /// <summary>
        /// Matches contact's phone number with phone number in <see cref="PersonalCard">PersonalCard</see> 
        /// </summary>
        /// <param name="contacts"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<Dictionary<Guid, Resource>> Match(PeopleResource.ConnectionsResource.ListRequest contacts, Guid userId)
        {
            string pageToken = "";
            var matchedPeopleIds = new Dictionary<Guid, Resource>();
            var matchedPeopleDbIds = _connectionRepository.Find(c => c.ReceiverUserId.Equals(userId)).ToDictionary(c => c.ReceiverUserId, c => c.Resource);
            do
            {
                if (pageToken != "")
                {
                    contacts.PageToken = pageToken;
                }

                var people = await contacts.ExecuteAsync();
                foreach (var contact in people.Connections)
                {
                    if (contact.PhoneNumbers.Count == 0)
                    {
                        continue;
                    }

                    var cards = await _personalCardRepository.GetAll();
                    foreach (var card in cards)
                    {
                        if (string.IsNullOrEmpty(card.Phone))
                        {
                            continue;
                        }

                        foreach (var phoneNumber in contact.PhoneNumbers)
                        {
                            if (phoneNumber.Value.Equals(card.Phone))
                            {
                                var resource = new Contactr.Models.Connection.Resources.Google
                                {
                                    ETag = contact.ETag,
                                    ResourceName = contact.ResourceName
                                };
                                matchedPeopleIds.Add(card.UserId, resource);
                            }
                        }
                    }
                }

                pageToken = people.NextPageToken;
            } while (pageToken != null);

            return matchedPeopleIds
                .Where(kvp => !matchedPeopleDbIds.ContainsKey(kvp.Key))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
        
        /// <summary>
        /// Reads out the user's Google Contact phonebook
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>List of Guids that user has no <see cref="Connection">connection</see> with</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task ReadGoogleContacts(Guid userId)
        {
            var peopleRequest = GetListRequest(userId);
            var matchesToBeCreated = await Match(peopleRequest, userId);
            await CreateConnection(matchesToBeCreated, userId);
        }
    }
}