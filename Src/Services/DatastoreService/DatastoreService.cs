using System;
using System.Collections.Generic;
using System.Linq;
using Contactr.Factories.Interfaces;
using Contactr.Models.Cards;
using Google.Apis.PeopleService.v1.Data;

namespace Contactr.Services.DatastoreService
{
    /// <summary>
    /// Class that is used to fill fields from <see cref="PersonalCard"/> and <see cref="BusinessCard"/>
    /// </summary>
    public class DatastoreService : IDatastoreService
    {
        private readonly IPeopleServiceFactory _peopleServiceFactory;
        
        private Person _person = null!;
        private PersonalCard _personalCard = null!;
        private IEnumerable<BusinessCard> _businessCards = null!;

        public DatastoreService(IPeopleServiceFactory peopleServiceFactory)
        {
            _peopleServiceFactory =
                peopleServiceFactory ?? throw new ArgumentNullException(nameof(peopleServiceFactory));
        }

        private void SetFields(PersonalCard personalCard, IEnumerable<BusinessCard> businessCards, Person person)
        {
            _person = person;
            _personalCard = personalCard;
            _businessCards = businessCards;
        }

        private void SetName()
        {
            if (string.IsNullOrEmpty(_personalCard.Firstname) || string.IsNullOrEmpty(_personalCard.Lastname))
            {
                return;
            }

            var names = new List<Name>
            {
                _peopleServiceFactory.CreateName(_personalCard.Firstname, _personalCard.Lastname,_personalCard.GetFullName())
            };

            _person.Names = names;
        }

        private void SetOrganizations()
        {
            if (!_businessCards.Any())
            {
                return;
            }

            var organizations = new List<Organization>();
            foreach (var businessCard in _businessCards)
            {
                organizations.Add(_peopleServiceFactory.CreateOrganization(businessCard.Company.Name, businessCard.JobTitle));
            }

            _person.Organizations = organizations;
        }

        private void SetEmails()
        {
            var emails = new List<EmailAddress>();
            if(!string.IsNullOrEmpty(_personalCard.Email))
                emails.Add(_peopleServiceFactory.CreateEmail("Personal", _personalCard.Email));
            if (_businessCards.Any())
            {
                foreach (var businessCard in _businessCards)
                {
                    if(!string.IsNullOrEmpty(businessCard.Email))
                        emails.Add(_peopleServiceFactory.CreateEmail(businessCard.Company.Name, businessCard.Email));
                }
            }

            _person.EmailAddresses = emails;
        }


        /// <summary>
        /// Calls all method to update fields from <see cref="PersonalCard"/> and <see cref="businessCards"/> in <see cref="Person"/>
        /// </summary>
        /// <param name="personalCard"></param>
        /// <param name="businessCards"></param>
        /// <param name="person"></param>
        /// <returns></returns>
        public Person UpdateFields(PersonalCard personalCard, IEnumerable<BusinessCard> businessCards, Person person)
        {
            SetFields(personalCard, businessCards, person);
            SetName();
            SetOrganizations();
            SetEmails();

            return person;
        }
    }
}