using System;
using System.Threading.Tasks;
using Contactr.DTOs.Cards;
using Contactr.Models.Cards;
using Contactr.Persistence;

namespace Contactr.Services.CardService
{
    public class CardService : ICardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
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
    }
}