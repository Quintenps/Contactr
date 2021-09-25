using System;
using System.Threading.Tasks;
using Contactr.DTOs.Cards;
using Contactr.Models.Cards;

namespace Contactr.Services.CardService
{
    public interface ICardService
    {
        public PersonalCard GetPersonalCard(Guid userId);
        public Task UpdatePersonalCard(Guid userId, PersonalCardDto personalCardDto);
    }
}