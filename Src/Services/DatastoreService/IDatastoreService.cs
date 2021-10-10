using System.Collections.Generic;
using Contactr.Models.Cards;
using Google.Apis.PeopleService.v1.Data;

namespace Contactr.Services.DatastoreService
{
    public interface IDatastoreService
    {
        public Person UpdateFields(PersonalCard personalCard, IEnumerable<BusinessCard> businessCards, Person person);
    }
}