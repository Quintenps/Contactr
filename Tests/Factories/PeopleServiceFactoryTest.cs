using System;
using Contactr.Factories;
using Contactr.Factories.Interfaces;
using Google.Apis.PeopleService.v1;
using Microsoft.Extensions.Configuration;
using Moq;
using Shouldly;
using Xunit;

namespace Contractr.Tests.Factories
{
    public class PeopleServiceFactoryTest
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly IPeopleServiceFactory _peopleServiceFactory;
        
        public PeopleServiceFactoryTest()
        {
            _configurationMock = new Mock<IConfiguration>();
            SetupMocks();
            
            _peopleServiceFactory = new PeopleServiceFactory(_configurationMock.Object);
        }

        private void SetupMocks()
        {
            Mock<IConfigurationSection> mockSectionClientId = new Mock<IConfigurationSection>();
            mockSectionClientId.Setup(x=>x.Value).Returns("clientid");
            Mock<IConfigurationSection> mockSectionClientSecret = new Mock<IConfigurationSection>();
            mockSectionClientSecret.Setup(x=>x.Value).Returns("clientsecret");
            
            _configurationMock.Setup(x => x.GetSection(PeopleServiceFactory.GOOGLE_OAUTH_CLIENTID)).Returns(mockSectionClientId.Object);
            _configurationMock.Setup(x => x.GetSection(PeopleServiceFactory.GOOGLE_OAUTH_CLIENTSECRET)).Returns(mockSectionClientSecret.Object);
        }
        
        [Fact]
        public void TestConstructor()
        {
            var constructor = new PeopleServiceFactory(_configurationMock.Object);
            Assert.NotNull(constructor);
        }

        [Fact]
        private void Test_Create_EmptyClientId_ThrowsException()
        {
            // Arrange
            var refreshToken = "12345";
            Mock<IConfigurationSection> mockSectionClientId = new();
            mockSectionClientId.Setup(x=>x.Value).Returns("");
            _configurationMock.Setup(x => x.GetSection(PeopleServiceFactory.GOOGLE_OAUTH_CLIENTID)).Returns(mockSectionClientId.Object);
            
            // Act & Assert
            Should.Throw<ArgumentNullException>(() => new PeopleServiceFactory(_configurationMock.Object).CreatePeopleServiceClient(refreshToken)).Message.ShouldBe("Google config not found (Parameter '_configuration')");
            
        }
        
        [Fact]
        private void Test_Create_EmptyClientSecret_ThrowsException()
        {
            // Arrange
            var refreshToken = "12345";
            Mock<IConfigurationSection> mockSectionClientSecret = new Mock<IConfigurationSection>();
            mockSectionClientSecret.Setup(x=>x.Value).Returns("");
            _configurationMock.Setup(x => x.GetSection(PeopleServiceFactory.GOOGLE_OAUTH_CLIENTSECRET)).Returns(mockSectionClientSecret.Object);
            
            // Act & Assert
            Should.Throw<ArgumentNullException>(() => new PeopleServiceFactory(_configurationMock.Object).CreatePeopleServiceClient(refreshToken)).Message.ShouldBe("Google config not found (Parameter '_configuration')");
        }

        [Fact]
        private void Test_Create_ReturnsPeopleServiceService()
        {
            // Arrange
            var refreshToken = "12345";
            
            // Act
            var result = _peopleServiceFactory.CreatePeopleServiceClient(refreshToken);
            
            // Assert
            result.ShouldBeOfType<PeopleServiceService>();
            result.ApplicationName.ShouldBeEquivalentTo("Contactr");
        }
    }
}