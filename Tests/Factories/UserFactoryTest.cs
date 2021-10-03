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
            var email = "test@contactr.local";
            string? avatar = null;
            var expectedUser = new User()
            {
                Email = email,
                Avatar = null
            };
            
            // Act
            var result = _userFactory.Create(email, avatar);

            // Assert
            result.ShouldBeOfType<User>();
            result.Email.ShouldBe(expectedUser.Email);
            result.Avatar.ShouldBe(expectedUser.Avatar);
        }
    }
}