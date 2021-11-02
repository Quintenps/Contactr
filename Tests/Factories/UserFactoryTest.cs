using System;
using Contactr.Factories;
using Contactr.Factories.Interfaces;
using Contactr.Models;
using Shouldly;
using Xunit;

namespace Contractr.Tests.Factories
{
    public class UserFactoryTest
    {
        private readonly IUserFactory _userFactory;

        public UserFactoryTest()
        {
            _userFactory = new UserFactory();
        }
        
        [Fact]
        public void TestConstructor()
        {
            var constructor = new UserFactory();
            Assert.NotNull(constructor);
        }

        [Fact]
        public void Test_Create_CreatesUser()
        {
            // Arrange
            var guid = new Guid("5BC7A31C-8690-4B62-8909-AD9485F61205");
            var auth0Id = "google-oauth2|1520432";
            var expectedUser = new User()
            {
                Id = guid,
                Auth0Id = auth0Id
            };
            
            // Act
            var result = _userFactory.Create(guid, auth0Id);

            // Assert
            result.ShouldBeOfType<User>();
            result.ShouldBeEquivalentTo(expectedUser);
        }
    }
}