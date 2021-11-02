using System;
using System.Text;
using Contactr.Factories;
using Contactr.Factories.Interfaces;
using Contactr.Models.Authentication;
using Contactr.Models.Enums;
using Microsoft.AspNetCore.DataProtection;
using Moq;
using Shouldly;
using Xunit;

namespace Contractr.Tests.Factories
{
    public class AuthenticationProviderFactoryTest
    {
        private Mock<IDataProtectionProvider> _dataProtectionProviderMock; 
        private readonly IAuthenticationProviderFactory _authenticationProviderFactory;

        public AuthenticationProviderFactoryTest()
        {
            _dataProtectionProviderMock = new Mock<IDataProtectionProvider>();
            SetupMocks();
            
            _authenticationProviderFactory = new AuthenticationProviderFactory(_dataProtectionProviderMock.Object);
        }

        private void SetupMocks()
        {
            Mock<IDataProtector> mockDataProtector = new();
            mockDataProtector.Setup(sut => sut.Protect(It.IsAny<byte[]>())).Returns(Encoding.UTF8.GetBytes("54321"));
            mockDataProtector.Setup(sut => sut.Unprotect(It.IsAny<byte[]>())).Returns(Encoding.UTF8.GetBytes("12345"));
            
            _dataProtectionProviderMock.Setup(s => s.CreateProtector(It.IsAny<string>())).Returns(mockDataProtector.Object);
            _dataProtectionProviderMock.Setup(x => x.CreateProtector(DataProtectionKeys.AuthProviderRefreshToken.ToString())).Returns(mockDataProtector.Object);
        }
        
        [Fact]
        public void Test_Constructor()
        {
            var constructor = new AuthenticationProviderFactory(_dataProtectionProviderMock.Object);
            Assert.NotNull(constructor);
        }

        [Fact]
        public void Test_Create_CreatesAuthenticationProvider()
        {
            // Arrange
            var userId = new Guid("DE98A149-08FB-4703-ACFD-51CBC125645B");
            var key = "890213809";
            var loginProvider = LoginProviders.Google;
            var refreshToken = "12345";
            
            var expectedAuthenticationProvider = new AuthenticationProvider()
            {
                UserId = userId,
                LoginProvider = loginProvider,
                RefreshToken = "NTQzMjE"
            };
            
            // Act
            var result = _authenticationProviderFactory.Create(userId, loginProvider, refreshToken);
            
            // Assert
            result.ShouldBeEquivalentTo(expectedAuthenticationProvider);

        }
    }
}