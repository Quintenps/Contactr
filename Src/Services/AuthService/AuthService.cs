using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Contactr.DTOs.Auth0;
using Contactr.DTOs.Cards;
using Contactr.Factories.Interfaces;
using Contactr.Models.Cards;
using Contactr.Models.Enums;
using Contactr.Persistence;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Newtonsoft.Json;
using User = Contactr.Models.User;

namespace Contactr.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IMemoryCache _cache;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserFactory _userFactory;
        private readonly ICardFactory _cardFactory;
        private readonly ILogger<AuthService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAuthenticationProviderFactory _authenticationProviderFactory;

        public AuthService(IMemoryCache cache, IUnitOfWork unitOfWork, IUserFactory userFactory, ICardFactory cardFactory, ILogger<AuthService> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory,
            IAuthenticationProviderFactory authenticationProviderFactory)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _userFactory = userFactory ?? throw new ArgumentNullException(nameof(userFactory));
            _cardFactory = cardFactory ?? throw new ArgumentNullException(nameof(cardFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _authenticationProviderFactory = authenticationProviderFactory ?? throw new ArgumentNullException(nameof(authenticationProviderFactory));
        }

        private void CheckIfUserAlreadyExists(Guid uuid)
        {
            if (_unitOfWork.UserRepository.Exists(u => u.Id.Equals(uuid)))
            {
                throw new ArgumentException($"User already exists with uuid {uuid}");
            }
        }

        private async Task<Token?> GetAuth0Token()
        {
            if (_cache.TryGetValue(CacheKeys.Auth0Token, out Token? authToken))
            {
                return authToken;
            }

            var auth0MachineDetails = new RequestTokenDto()
            {
                ClientId = _configuration.GetValue<string>("Auth0Machine:clientId"),
                ClientSecret = _configuration.GetValue<string>("Auth0Machine:clientSecret"),
                Audience = _configuration.GetValue<string>("Auth0Machine:audience"),
                GrantType = "client_credentials" 
            };

            var client = _httpClientFactory.CreateClient();
            var tokenUrl = _configuration.GetValue<string>("Auth0Machine:token");
            HttpResponseMessage response = await client.PostAsync(tokenUrl, new StringContent(
                JsonConvert.SerializeObject(auth0MachineDetails), Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Couldn't get auth0 token");
                _logger.LogError(await response.Content.ReadAsStringAsync());
                throw new HttpRequestException();
            }
            var responseContent = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<Token>(responseContent);
            _cache.Set(CacheKeys.Auth0Token, token, DateTimeOffset.Now.AddHours(23));

            return token;
        } 
        
        private async Task<UserDto?> LookupAuth0User(string auth0Id, Token? token)
        {
            var userInfoUrl = _configuration.GetValue<string>("Auth0Machine:audience") + $"users/{auth0Id}";
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            var response = await client.GetAsync(userInfoUrl);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Couldn't lookup auth0 user {auth0Id}");
                _logger.LogError(await response.Content.ReadAsStringAsync());
                throw new HttpRequestException();
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserDto>(responseContent);
        }

        private static void FillPersonalCardFromAuth0User(UserDto auth0User, PersonalCard personalCard)
        {
            personalCard.Email = auth0User.Email;
            personalCard.Firstname = auth0User.GivenName;
            personalCard.Lastname = auth0User.Name;
        }

        private void CreateAuthenticationProviders(UserDto auth0User, Guid userId)
        {
            foreach (var identity in auth0User.Identities)
            {
                if(string.IsNullOrEmpty(identity.RefreshToken))
                    continue;

                switch (identity.Provider)
                {
                    case "google-oauth2":
                        var authenticationProvider = _authenticationProviderFactory.Create(userId, LoginProviders.Google, identity.RefreshToken);
                        _unitOfWork.AuthenticationProviderRepository.Add(authenticationProvider);
                        break;
                }
            }
        }
        
        public async Task<User> CreateUser(Guid uuid, string auth0Id)
        {
            CheckIfUserAlreadyExists(uuid);
            var user = _userFactory.Create(uuid, auth0Id);
            user.PersonalCard = _cardFactory.CreatePersonalCard(uuid);
            _unitOfWork.UserRepository.Add(user);

            var token = await GetAuth0Token();
            var auth0User = await LookupAuth0User(user.Auth0Id, token);
            if (auth0User != null)
            {
                FillPersonalCardFromAuth0User(auth0User, user.PersonalCard);
                CreateAuthenticationProviders(auth0User, user.Id);
            }

            await _unitOfWork.Save();

            return user;
        }
    }
}