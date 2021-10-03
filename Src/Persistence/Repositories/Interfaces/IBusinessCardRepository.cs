using System;
using System.Collections.Generic;
using Contactr.Models.Cards;

namespace Contactr.Persistence.Repositories.Interfaces
{
    public interface IBusinessCardRepository : IRepository<BusinessCard>
    {
        public IEnumerable<BusinessCard> GetAll(Guid userId);
        public BusinessCard? Get(Guid userId, Guid cardId);
    }
}