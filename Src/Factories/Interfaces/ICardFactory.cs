using System;
using Contactr.DTOs.Cards;
using Contactr.Models.Cards;

namespace Contactr.Factories.Interfaces
{
    public interface ICardFactory
    {
        public PersonalCard CreatePersonalCard(Guid userId);
        public PersonalCard CreatePersonalCard(Guid userId, PersonalCardDto personalCardDto);
        public BusinessCard CreateBusinessCard(Guid userId);
    }
}