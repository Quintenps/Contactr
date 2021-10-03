using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contactr.Factories.Interfaces;
using Contactr.Models.Authentication;
using Contactr.Models.Cards;
using Contactr.Models.Connection;
using Contactr.Models.Enums;
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
    public class ConnectionService : IConnectionService
    {
        private readonly IPeopleServiceFactory _peopleServiceFactory;
        private readonly IConnectionFactory _connectionFactory;
        private readonly IAuthenticationProviderRepository _authenticationProviderRepository;
        private readonly IDataProtector _dataProtector;
        private readonly IPersonalCardRepository _personalCardRepository;
        private readonly IConnectionRepository _connectionRepository;
        private readonly ILogger<ConnectionService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        private static string PersonFields = "names,emailAddresses,phoneNumbers";

        public ConnectionService(IPeopleServiceFactory peopleServiceFactory, IDataProtectionProvider dataProtectionProvider, ILogger<ConnectionService> logger, IAuthenticationProviderRepository authenticationProviderRepository, IUnitOfWork unitOfWork,
            IPersonalCardRepository personalCardRepository, IConnectionRepository connectionRepository, IConnectionFactory connectionFactory)
        {
            _peopleServiceFactory = peopleServiceFactory ?? throw new ArgumentNullException(nameof(peopleServiceFactory));
            _authenticationProviderRepository = authenticationProviderRepository ?? throw new ArgumentNullException(nameof(authenticationProviderRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _personalCardRepository = personalCardRepository ?? throw new ArgumentNullException(nameof(personalCardRepository));
            _connectionRepository = connectionRepository ?? throw new ArgumentNullException(nameof(connectionRepository));
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _dataProtector = dataProtectionProvider.CreateProtector(DataProtectionKeys.AuthProviderRefreshToken.ToString()) ?? throw new ArgumentNullException(nameof(dataProtectionProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Creates <see cref="Connection" /> and saves it to the database
        /// </summary>
        /// <param name="matches"></param>
        /// <param name="userId"></param>
        private async Task CreateConnection(Dictionary<Guid, string> matches, Guid userId)
        {
            _logger.LogInformation($"Creating {matches.Count} matches for userId {userId}");
            foreach (var pair in matches)
            {
                _logger.LogInformation($"Creating connection from userId {pair.Key} to {userId}");
                var connection = _connectionFactory.Create(userId, pair.Key, pair.Value);
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
        private async Task<Dictionary<Guid, string>> Match(PeopleResource.ConnectionsResource.ListRequest contacts, Guid userId)
        {
            string pageToken = "";
            var matchedPeopleIds = new Dictionary<Guid, string>();
            var matchedPeopleDbIds = _connectionRepository.Find(c => c.ReceiverUserId.Equals(userId)).ToDictionary(c => c.ReceiverUserId, c => c.ReceiverUserId);
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
                                matchedPeopleIds.Add(card.UserId, contact.ETag);
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
        
        private AuthenticationProvider? GetAuthenticationProvider(Guid userId)
        {
            var authenticationProvider = _authenticationProviderRepository.SingleOrDefault(ap => ap.UserId.Equals(userId));
            if (authenticationProvider is null)
            {
                _logger.LogError($"Authentication provider not found for userId {userId}");
                throw new ArgumentException("Authenticationprovider not found");
            }

            return authenticationProvider;
        }
        
        /// <summary>
        /// Reads out the user's Google Contact phonebook
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>List of Guids that user has no <see cref="Connection">connection</see> with</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task ReadGoogleContacts(Guid userId)
        {
            var authenticationProvider = GetAuthenticationProvider(userId);
            var service = _peopleServiceFactory.Create(_dataProtector.Unprotect(authenticationProvider.RefreshToken));
            PeopleResource.ConnectionsResource.ListRequest peopleRequest = service.People.Connections.List("people/me");
            peopleRequest.PersonFields = PersonFields;

            var matchesToBeCreated = await Match(peopleRequest, userId);
            await CreateConnection(matchesToBeCreated, userId);
        }
    }
}