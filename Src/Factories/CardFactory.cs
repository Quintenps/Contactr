using System;
using Contactr.DTOs.Cards;
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
        
        public PersonalCard CreatePersonalCard(Guid userId, PersonalCardDto personalCardDto)
        {
            return new PersonalCard()
            {
                UserId = userId,
                Address = personalCardDto.Address,
                Birthday = personalCardDto.Birthday,
                City = personalCardDto.City,
                Email = personalCardDto.Email,
                Country = personalCardDto.Country,
                Gender = personalCardDto.Gender,
                Firstname = personalCardDto.Firstname,
                Lastname = personalCardDto.Lastname,
                Postalcode = personalCardDto.Postalcode,
                Phone = personalCardDto.Phone
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