using System;
using Contactr.Factories;
using Contactr.Factories.Interfaces;
using Contactr.Models.Cards;
using Xunit;
using Shouldly;

namespace Contractr.Tests.Factories
{
    public class CardFactoryTest
    {
        private readonly ICardFactory _cardFactory;

        public CardFactoryTest()
        {
            _cardFactory = new CardFactory();
        }
        
        [Fact]
        public void TestConstructor()
        {
            var constructor = new CardFactory();
            Assert.NotNull(constructor);
        }

        [Fact]
        public void Test_CreatePersonalCard_CreatesCard()
        {
            // Arrange
            var userId = new Guid("D1525D9A-8D76-4673-A7D3-B362CDA7FF97");
            var expectedPersonalCard = new PersonalCard()
            {
                UserId = userId
            };

            // Act
            var result = _cardFactory.CreatePersonalCard(userId);

            // Assert
            result.ShouldBeEquivalentTo(expectedPersonalCard);
        }

        [Fact]
        public void Test_CreateBusinessCard_CreatesCard()
        {
            // Arrange
            var userId = new Guid("1FD035D0-2E59-41B6-B629-19DEE2C9A644");

            // Act
            var result = _cardFactory.CreateBusinessCard(userId);

            // Assert
            result.ShouldBeOfType<BusinessCard>();
            result.UserId.ShouldBe(userId);
        }
    }
}