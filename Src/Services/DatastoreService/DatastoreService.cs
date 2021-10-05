using System;
using System.Collections.Generic;
using Contactr.Factories.Interfaces;
using Contactr.Models.Cards;
using Contactr.Services.CardService;
using Google.Apis.PeopleService.v1.Data;

namespace Contactr.Services.DatastoreService
{
    public class DatastoreService : IDatastoreService
    {
        private readonly ICardService _cardService;
        private readonly IPeopleServiceFactory _peopleServiceFactory;
        //
        private Person _person;
        //
        public DatastoreService(ICardService cardService, IPeopleServiceFactory peopleServiceFactory)
        {
        _cardService = cardService ?? throw new ArgumentNullException(nameof(cardService));
        _peopleServiceFactory = peopleServiceFactory ?? throw new ArgumentNullException(nameof(peopleServiceFactory));
        }
        //
        // private void SetName(Person person)
        // {
        //     if (string.IsNullOrEmpty(_personalCard.Firstname) && string.IsNullOrEmpty(_personalCard.Lastname))
        //     {
        //         return;
        //     }
        //
        //     var name = _peopleServiceFactory.CreateName(_personalCard.Firstname, _personalCard.Lastname, _personalCard.GetFullName());
        //     person.Names.Add(name);
        // }
        //
        //
        public void MakeRequest(Guid userId, Person person)
        {
            throw new NotImplementedException();
            // _person = person;
            // LoadCards(userId);
            // SetName();
        }
    }
}