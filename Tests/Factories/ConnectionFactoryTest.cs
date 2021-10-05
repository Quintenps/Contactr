using System;
using Contactr.Factories;
using Contactr.Factories.Interfaces;
using Contactr.Models.Connection;
using Contactr.Models.Enums;
using Shouldly;
using Xunit;

namespace Contractr.Tests.Factories
{
    public class ConnectionFactoryTest
    {
        private readonly IConnectionFactory _connectionFactory;

        public ConnectionFactoryTest()
        {
            _connectionFactory = new ConnectionFactory();
        }
        
        [Fact]
        public void TestConstructor()
        {
            var constructor = new ConnectionFactory();
            Assert.NotNull(constructor);
        }
        
        [Fact]
        public void Test_Create_CreatesConnection()
        {
            // Arrange
            var etag = "dopwakdopawk";
            var resourceName = "kdopwakdopawkd";
            var userId = new Guid("51848B31-8FFC-4311-AFCB-D4F6002E004C");
            var receiverUserId = new Guid("BF07A60D-8178-4899-86A7-AC2B02FD3B15");
            var expectedConnection = new Connection()
            {
                Resource = new Contactr.Models.Connection.Resources.Google
                {
                    ETag = etag,
                    ResourceName = resourceName,
                    Provider = SyncProviders.Google
                },
                ReceiverUserId = receiverUserId,
                SenderUserId = userId
            };

            // Act
            var result = _connectionFactory.Create(receiverUserId, userId, etag, resourceName);

            // Assert
            result.Resource.ShouldBeEquivalentTo(expectedConnection.Resource);
            result.ReceiverUserId.ShouldBe(expectedConnection.ReceiverUserId);
            result.SenderUserId.ShouldBe(expectedConnection.SenderUserId);
        }
    }
}