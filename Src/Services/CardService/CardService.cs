using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contactr.DTOs.Cards;
using Contactr.Factories.Interfaces;
using Contactr.Models.Cards;
using Contactr.Persistence;

namespace Contactr.Services.CardService
{
    public class CardService : ICardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICardFactory _cardFactory;

        public CardService(IUnitOfWork unitOfWork, ICardFactory cardFactory)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _cardFactory = cardFactory ?? throw new ArgumentNullException(nameof(cardFactory));
        }

        public PersonalCard GetPersonalCard(Guid userId)
        {
            return _unitOfWork.PersonalCardRepository.SingleOrDefault(pc => pc.UserId.Equals(userId));
        }

        public async Task UpdatePersonalCard(Guid userId, PersonalCardDto personalCardDto)
        {
            var card = _unitOfWork.PersonalCardRepository.SingleOrDefault(pc => pc.UserId.Equals(userId));
            card.Firstname = personalCardDto.Firstname;
            card.Lastname = personalCardDto.Lastname;
            card.Gender = personalCardDto.Gender;
            card.Birthday = personalCardDto.Birthday;
            card.Email = personalCardDto.Email;
            card.Phone = personalCardDto.Phone;
            card.Address = personalCardDto.Address;
            card.Country = personalCardDto.Country;
            card.Postalcode = personalCardDto.Postalcode;
            card.City = personalCardDto.City;

            await _unitOfWork.Save();
        }

        private void UpdateBusinessCard(BusinessCard card, BusinessCardCreateDto businessCardDto)
        {
            card.CompanyId = businessCardDto.CompanyId;
            card.AddressId = businessCardDto.AddressId;
            card.JobTitle = businessCardDto.JobTitle;
            card.Email = businessCardDto.Email;
            card.Phone = businessCardDto.Phone;
        }

        private static void CheckIfBusinessCardExists(BusinessCard businessCard)
        {
            if (businessCard is null)
                throw new ArgumentException("Business card not found");
        }
        
        public BusinessCard? GetBusinessCard(Guid userId, Guid cardId)
        {
            return _unitOfWork.BusinessCardRepository.Get(userId, cardId);
        }

        public IEnumerable<BusinessCard> GetBusinessCards(Guid userId)
        {
            return _unitOfWork.BusinessCardRepository.GetAll(userId);
        }

        public async Task CreateBusinessCard(Guid userId, BusinessCardCreateDto businessCardDto)
        {
            var card = _cardFactory.CreateBusinessCard(userId);
            UpdateBusinessCard(card, businessCardDto);
            _unitOfWork.BusinessCardRepository.Add(card);
            await _unitOfWork.Save();
        }
        
        public async Task UpdateBusinessCard(Guid userId, Guid cardId, BusinessCardCreateDto businessCardDto)
        {
            var card = _unitOfWork.BusinessCardRepository.SingleOrDefault(pc => pc.UserId.Equals(userId) && pc.Id.Equals(cardId));
            CheckIfBusinessCardExists(card);
            UpdateBusinessCard(card, businessCardDto);
            await _unitOfWork.Save();
        }
        
        public async Task DeleteBusinessCard(Guid userId, Guid cardId)
        {
            var card = _unitOfWork.BusinessCardRepository.SingleOrDefault(pc => pc.UserId.Equals(userId) && pc.Id.Equals(cardId));
            CheckIfBusinessCardExists(card);
            _unitOfWork.BusinessCardRepository.Remove(card);
            await _unitOfWork.Save();
        }
    }
}