using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Contactr.DTOs;
using Contactr.DTOs.Cards;
using Contactr.Factories.Interfaces;
using Contactr.Models.Cards;
using Contactr.Models.Enums;
using Contactr.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Contactr.Services.CardService
{
    public class CardService : ICardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICardFactory _cardFactory;
        private readonly ILogger<CardService> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CardService(IUnitOfWork unitOfWork, ICardFactory cardFactory, ILogger<CardService> logger, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _cardFactory = cardFactory ?? throw new ArgumentNullException(nameof(cardFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
        }

        private void UpdatePersonalCard(PersonalCard card, PersonalCardDto personalCardDto)
        {
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
        }

        private void UpdateBusinessCard(BusinessCard card, BusinessCardCreateDto businessCardDto)
        {
            card.CompanyId = businessCardDto.CompanyId;
            card.AddressId = businessCardDto.AddressId;
            card.JobTitle = businessCardDto.JobTitle;
            card.Email = businessCardDto.Email;
            card.Phone = businessCardDto.Phone;
        }

        private void CheckIfPersonalCardAlreadyExists(Guid userId)
        {
            if (_unitOfWork.PersonalCardRepository.Exists(pc => pc.UserId.Equals(userId)))
            {
                throw new ArgumentException("PersonalCard already exists");
            }
        }

        private static void CheckIfPersonalCardExists(PersonalCard personalCard)
        {
            if (personalCard is null)
                throw new ArgumentException("Personal card not found");
        }

        private static void CheckIfBusinessCardExists(BusinessCard businessCard)
        {
            if (businessCard is null)
                throw new ArgumentException("Business card not found");
        }

        public PersonalCard GetPersonalCard(Guid userId)
        {
            return _unitOfWork.PersonalCardRepository.SingleOrDefault(pc => pc.UserId.Equals(userId));
        }

        public async Task CreatePersonalCard(Guid userId, PersonalCardDto personalCardDto)
        {
            CheckIfPersonalCardAlreadyExists(userId);
            var card = _cardFactory.CreatePersonalCard(userId);
            UpdatePersonalCard(card, personalCardDto);
            _unitOfWork.PersonalCardRepository.Add(card);
            await _unitOfWork.Save();
        }

        public async Task UploadAvatar(Guid userId, IFormFile avatar)
        {
            // Validator
            var card = _unitOfWork.PersonalCardRepository.SingleOrDefault(pc => pc.UserId.Equals(userId));
            CheckIfPersonalCardExists(card);
            var path = _webHostEnvironment.WebRootPath + Images.PersonalCardFolder;
            var filename = Path.GetRandomFileName() + ".jpg";

            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                await using var fileStream = File.Create(path + filename);
                await avatar.CopyToAsync(fileStream);
                await fileStream.FlushAsync();

                // Cleanup old image
                if (!string.IsNullOrEmpty(card.Avatar))
                    File.Delete(path + card.Avatar);

                card.Avatar = filename;
                await _unitOfWork.Save();
            }
            catch (Exception e)
            {
                _logger.LogWarning("Couldn't upload avatar - " + e.Message);
            }
        }

        public async Task UpdatePersonalCard(Guid userId, PersonalCardDto personalCardDto)
        {
            var card = _unitOfWork.PersonalCardRepository.SingleOrDefault(pc => pc.UserId.Equals(userId));
            CheckIfPersonalCardExists(card);
            UpdatePersonalCard(card, personalCardDto);
            await _unitOfWork.Save();
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
            // TODO: Validator
            var card = _cardFactory.CreateBusinessCard(userId);
            UpdateBusinessCard(card, businessCardDto);
            _unitOfWork.BusinessCardRepository.Add(card);
            await _unitOfWork.Save();
        }

        public async Task UpdateBusinessCard(Guid userId, Guid cardId, BusinessCardCreateDto businessCardDto)
        {
            // TODO: Validator
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