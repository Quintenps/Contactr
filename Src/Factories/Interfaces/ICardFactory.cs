using System;
using Contactr.Models.Cards;

namespace Contactr.Factories.Interfaces
{
    public interface ICardFactory
    {
        public PersonalCard CreatePersonalCard(Guid userId);
        public BusinessCard CreateBusinessCard(Guid userId);
    }
}