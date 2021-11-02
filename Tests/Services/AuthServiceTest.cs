using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Contactr.DTOs.Auth0;
using Contactr.Factories.Interfaces;
using Contactr.Models;
using Contactr.Models.Authentication;
using Contactr.Models.Cards;
using Contactr.Models.Enums;
using Contactr.Persistence;
using Contactr.Services.AuthService;
using Google.Apis.Auth;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace Contractr.Tests.Services
{
    public class AuthServiceTest
    {
        private readonly IMemoryCache _memoryCache;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUserFactory> _userFactoryMock;
        private readonly Mock<ICardFactory> _cardFactoryMock;
        private readonly Mock<ILogger<AuthService>> _loggerMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<IAuthenticationProviderFactory> _authenticationProviderFactoryMock;

        private readonly AuthService _authService;

        public AuthServiceTest()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userFactoryMock = new Mock<IUserFactory>();
            _cardFactoryMock = new Mock<ICardFactory>();
            _loggerMock = new Mock<ILogger<AuthService>>();
            _configurationMock = new Mock<IConfiguration>();
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _authenticationProviderFactoryMock = new Mock<IAuthenticationProviderFactory>();
            SetupMocks();

            _authService = new AuthService(_memoryCache, _unitOfWorkMock.Object, _userFactoryMock.Object, _cardFactoryMock.Object, _loggerMock.Object, _configurationMock.Object, _httpClientFactoryMock.Object,
                _authenticationProviderFactoryMock.Object);
        }

        private void SetupMocks()
        {
            
        }

        [Fact]
        public void Test_Constructor()
        {
            var constructor = new AuthService(_memoryCache, _unitOfWorkMock.Object, _userFactoryMock.Object, _cardFactoryMock.Object, _loggerMock.Object, _configurationMock.Object, _httpClientFactoryMock.Object,
                _authenticationProviderFactoryMock.Object);
            Assert.NotNull(constructor);
        }

        [Fact]
        public void Test_ConstructorFunction_throws_Exception()
        {
            Should.Throw<ArgumentNullException>(() => new AuthService(null, _unitOfWorkMock.Object, _userFactoryMock.Object, _cardFactoryMock.Object, _loggerMock.Object, _configurationMock.Object, _httpClientFactoryMock.Object,
                _authenticationProviderFactoryMock.Object));

            Should.Throw<ArgumentNullException>(() => new AuthService(_memoryCache, null, _userFactoryMock.Object, _cardFactoryMock.Object, _loggerMock.Object, _configurationMock.Object, _httpClientFactoryMock.Object,
                _authenticationProviderFactoryMock.Object));

            Should.Throw<ArgumentNullException>(() => new AuthService(_memoryCache, _unitOfWorkMock.Object, null, _cardFactoryMock.Object, _loggerMock.Object, _configurationMock.Object, _httpClientFactoryMock.Object,
                _authenticationProviderFactoryMock.Object));

            Should.Throw<ArgumentNullException>(() => new AuthService(_memoryCache, _unitOfWorkMock.Object, _userFactoryMock.Object, null, _loggerMock.Object, _configurationMock.Object, _httpClientFactoryMock.Object,
                _authenticationProviderFactoryMock.Object));

            Should.Throw<ArgumentNullException>(() => new AuthService(_memoryCache, _unitOfWorkMock.Object, _userFactoryMock.Object, _cardFactoryMock.Object, null, _configurationMock.Object, _httpClientFactoryMock.Object,
                _authenticationProviderFactoryMock.Object));

            Should.Throw<ArgumentNullException>(() => new AuthService(_memoryCache, _unitOfWorkMock.Object, _userFactoryMock.Object, _cardFactoryMock.Object, _loggerMock.Object, null, _httpClientFactoryMock.Object,
                _authenticationProviderFactoryMock.Object));
            
            Should.Throw<ArgumentNullException>(() => new AuthService(_memoryCache, _unitOfWorkMock.Object, _userFactoryMock.Object, _cardFactoryMock.Object, _loggerMock.Object, _configurationMock.Object, null,
                _authenticationProviderFactoryMock.Object));
            
            Should.Throw<ArgumentNullException>(() => new AuthService(_memoryCache, _unitOfWorkMock.Object, _userFactoryMock.Object, _cardFactoryMock.Object, _loggerMock.Object, _configurationMock.Object, _httpClientFactoryMock.Object,
                null));
        }
        
        [Fact]
        public async Task Test_Login_CreatesUserAndAuthenticationProvider()
        {
            // Arrange
            var userId = new Guid("B8CCE1A5-4EC6-4D68-826C-2272FC792CA6");
            const string auth0Id = "google-oauth2|1520432";

            var auth0User = new UserDto()
            {
                Email = "test@contactr.com",
                GivenName = "John",
                Identities = new List<IdentityDto>()
                {
                    new()
                    {
                        Provider = "google-oauth2",
                        RefreshToken = "ko3p12i309120-381293"
                    }
                }
            };
            var userToBeCreated = new User()
            {
                Id = userId,
                Auth0Id = auth0Id
            };
            var personalCardToBeCreated = new PersonalCard()
            {
                UserId = userId,
                Email = auth0User.Email
            };
            var authToken = new Token()
            {
                AccessToken = "12345",
                TokenType = "Bearer"
            };
            var auth0UserResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(auth0User))
            };
            var url = "https://auth0";
            var auth0UrlConfiguration = new Mock<IConfigurationSection>();

            _unitOfWorkMock.Setup(x => x.UserRepository.Exists(u => u.Equals(userId))).Returns(false);
            _unitOfWorkMock.Setup(x => x.AuthenticationProviderRepository.Add(It.IsAny<AuthenticationProvider>()));
            _userFactoryMock.Setup(x => x.Create(userId, auth0Id)).Returns(userToBeCreated);
            _cardFactoryMock.Setup(x => x.CreatePersonalCard(userId)).Returns(personalCardToBeCreated);
            _configurationMock.Setup(x => x.GetSection("Auth0Machine:audience")).Returns(auth0UrlConfiguration.Object);
            auth0UrlConfiguration.Setup(x => x.Value).Returns(url);
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(r => r.RequestUri.ToString().StartsWith(url)), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(auth0UserResponse);
            _memoryCache.Set(CacheKeys.Auth0Token, authToken);
            var client = new HttpClient(mockHttpMessageHandler.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
            
            
            // Act
            await _authService.CreateUser(userId, auth0Id);

            // Assert
            _unitOfWorkMock.Verify(x => x.AuthenticationProviderRepository.Add(It.IsAny<AuthenticationProvider>()));
            _unitOfWorkMock.Verify(x => x.UserRepository.Add(userToBeCreated));
            _unitOfWorkMock.Verify(x => x.Save());
        }
        
    }
}