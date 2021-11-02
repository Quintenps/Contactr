using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contactr.DTOs.Cards;
using Contactr.Models.Cards;
using Microsoft.AspNetCore.Http;

namespace Contactr.Services.CardService
{
    public interface ICardService
    {
        public PersonalCard GetPersonalCard(Guid userId);
        public Task CreatePersonalCard(Guid userId, PersonalCardDto personalCardDto);
        public Task UpdatePersonalCard(Guid userId, PersonalCardDto personalCardDto);
        public Task UploadAvatar(Guid userId, IFormFile avatar);
        public BusinessCard? GetBusinessCard(Guid userId, Guid cardId);
        public IEnumerable<BusinessCard> GetBusinessCards(Guid userId);
        public Task CreateBusinessCard(Guid userId, BusinessCardCreateDto businessCardDto);
        public Task UpdateBusinessCard(Guid userId, Guid cardId, BusinessCardCreateDto businessCardDto);
        public Task DeleteBusinessCard(Guid userId, Guid cardId);
    }
}