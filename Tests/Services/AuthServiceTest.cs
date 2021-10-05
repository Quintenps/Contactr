using System;
using System.Threading.Tasks;
using Contactr.Factories.Interfaces;
using Contactr.Models;
using Contactr.Models.Authentication;
using Contactr.Models.Cards;
using Contactr.Models.Enums;
using Contactr.Persistence;
using Contactr.Services.AuthService;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using Xunit;

namespace Contractr.Tests.Services
{
    public class AuthServiceTest
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<AuthService>> _loggerMock;
        private readonly Mock<IUserFactory> _userFactoryMock;
        private readonly Mock<ICardFactory> _cardFactoryMock;
        private readonly Mock<IAuthenticationProviderFactory> _authenticationProviderFactoryMock;
        private readonly AuthService _authService;

        public AuthServiceTest()
        {
            _configurationMock = new Mock<IConfiguration>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<AuthService>>();
            _userFactoryMock = new Mock<IUserFactory>();
            _configurationMock = new Mock<IConfiguration>();
            _cardFactoryMock = new Mock<ICardFactory>();
            _authenticationProviderFactoryMock = new Mock<IAuthenticationProviderFactory>();
            SetupMocks();
            
            _authService = new AuthService(_configurationMock.Object, _unitOfWorkMock.Object, _loggerMock.Object, _userFactoryMock.Object, _cardFactoryMock.Object, _authenticationProviderFactoryMock.Object);
        }

        private void SetupMocks()
        {
            Mock<IConfigurationSection> mockSectionClientId = new();
            mockSectionClientId.Setup(x=>x.Value).Returns("dkpoawkd09kdlopawkf2");
            _configurationMock.Setup(x => x.GetSection(AuthService.JWT_TOKEN)).Returns(mockSectionClientId.Object);
        }

        [Fact]
        public void Test_Constructor()
        {
            var constructor = new AuthService(_configurationMock.Object, _unitOfWorkMock.Object, _loggerMock.Object, _userFactoryMock.Object, _cardFactoryMock.Object, _authenticationProviderFactoryMock.Object);
            Assert.NotNull(constructor);
        }

        [Fact]
        public void Test_ConstructorFunction_throws_Exception()
        {
            Should.Throw<ArgumentNullException>(() => new AuthService(
                null!, _unitOfWorkMock.Object, _loggerMock.Object, _userFactoryMock.Object, _cardFactoryMock.Object, _authenticationProviderFactoryMock.Object
            ));

            Should.Throw<ArgumentNullException>(() => new AuthService(
                _configurationMock.Object, null!, _loggerMock.Object, _userFactoryMock.Object, _cardFactoryMock.Object, _authenticationProviderFactoryMock.Object
            ));

            Should.Throw<ArgumentNullException>(() => new AuthService(
                _configurationMock.Object, _unitOfWorkMock.Object, null!, _userFactoryMock.Object, _cardFactoryMock.Object, _authenticationProviderFactoryMock.Object
            ));

            Should.Throw<ArgumentNullException>(() => new AuthService(
                _configurationMock.Object, _unitOfWorkMock.Object, _loggerMock.Object, null!, _cardFactoryMock.Object, _authenticationProviderFactoryMock.Object
            ));

            Should.Throw<ArgumentNullException>(() => new AuthService(
                _configurationMock.Object, _unitOfWorkMock.Object, _loggerMock.Object, _userFactoryMock.Object, null!, _authenticationProviderFactoryMock.Object
            ));

            Should.Throw<ArgumentNullException>(() => new AuthService(
                _configurationMock.Object, _unitOfWorkMock.Object, _loggerMock.Object, _userFactoryMock.Object, _cardFactoryMock.Object, null!
            ));
        }

        [Fact]
        public void Test_login_NoJwtToken_ThrowsArgumentException()
        {
            // Arrange
            Mock<IConfigurationSection> mockSectionClientId = new();
            mockSectionClientId.Setup(x=>x.Value).Returns("");
            _configurationMock.Setup(x => x.GetSection(AuthService.JWT_TOKEN)).Returns(mockSectionClientId.Object);
            
            // Act & Assert
            Should.Throw<ArgumentException>(() =>
                new AuthService(_configurationMock.Object, _unitOfWorkMock.Object, _loggerMock.Object, _userFactoryMock.Object, _cardFactoryMock.Object, _authenticationProviderFactoryMock.Object));
        }

        [Fact]
        public async Task Test_Login_CreatesUserAndToken()
        {
            // Arrange
            var refreshToken = "90312jed92";
            var payload = new GoogleJsonWebSignature.Payload()
            {
                Email = "tester@contactr.local",
                Subject = "12345"
            };
            var userToBeCreated = new User()
            {
                Id = Guid.Empty,
                Email = payload.Email
            };
            var personalCardToBeCreated = new PersonalCard()
            {
                UserId = userToBeCreated.Id,
                Email = userToBeCreated.Email
            };
            var authenticationProviderToBeCreated = new AuthenticationProvider()
            {
                Key = "12345",
                UserId = userToBeCreated.Id,
                LoginProvider = LoginProviders.Google,
                RefreshToken = "6789"
            };
            _unitOfWorkMock.Setup(u => u.AuthenticationProviderRepository.GetProviderWithUserOrDefault("12345")).Returns<AuthenticationProvider>(null);
            _authenticationProviderFactoryMock.Setup(x => x.Create(userToBeCreated.Id, payload.Subject, LoginProviders.Google, refreshToken)).Returns(authenticationProviderToBeCreated);
            _unitOfWorkMock.Setup(u => u.PersonalCardRepository.Add(personalCardToBeCreated));
            _unitOfWorkMock.Setup(u => u.UserRepository.Add(userToBeCreated));
            _userFactoryMock.Setup(x => x.Create(payload.Email, null)).Returns(userToBeCreated);
            _cardFactoryMock.Setup(x => x.CreatePersonalCard(userToBeCreated.Id)).Returns(personalCardToBeCreated);
            

            // Act
            await _authService.Login(payload, refreshToken);
            
            // Assert
            _unitOfWorkMock.Verify(x => x.AuthenticationProviderRepository.Add(authenticationProviderToBeCreated));
            _unitOfWorkMock.Verify(x => x.PersonalCardRepository.Add(personalCardToBeCreated));
            _unitOfWorkMock.Verify(x => x.UserRepository.Add(userToBeCreated));
            _unitOfWorkMock.Verify(x => x.Save());
        }

        [Fact]
        public async Task Test_Login_CreatesToken()
        {
            // Arrange
            var refreshToken = "kpo518f9r2";
            var payload = new GoogleJsonWebSignature.Payload()
            {
                Email = "tester@contactr.local",
                Subject = "12345"
            };
            var authenticationProvider = new AuthenticationProvider()
            {
                Key = "12345",
                UserId = Guid.Empty,
                User = new User()
                {
                    Id = Guid.Empty,
                    Email = "tester@contactr.local"
                },
                LoginProvider = LoginProviders.Google,
                RefreshToken = refreshToken
            };
            _unitOfWorkMock.Setup(u => u.AuthenticationProviderRepository.GetProviderWithUserOrDefault("12345")).Returns(authenticationProvider);

            // Act
            var result = await _authService.Login(payload, refreshToken);
            
            // Assert
            result.ShouldBeOfType<string>();
        }
    }
}