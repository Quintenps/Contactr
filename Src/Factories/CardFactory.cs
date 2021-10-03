using System;
using Contactr.Factories.Interfaces;
using Contactr.Models.Cards;

namespace Contactr.Factories
{
    public class CardFactory : ICardFactory
    {
        public PersonalCard CreatePersonalCard(Guid userId)
        {
            return new PersonalCard()
            {
                UserId = userId
            };
        }

        public BusinessCard CreateBusinessCard(Guid userId)
        {
            return new BusinessCard()
            {
                Id = Guid.NewGuid(),
                UserId = userId
            };
        }
    }
}