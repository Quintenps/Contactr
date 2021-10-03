using System;
using System.Collections.Generic;
using System.Linq;
using Contactr.Models.Cards;
using Contactr.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Contactr.Persistence.Repositories
{
    public class BusinessCardRepository : Repository<BusinessCard>, IBusinessCardRepository
    {
        public BusinessCardRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public IEnumerable<BusinessCard> GetAll(Guid userId)
        {
            return mUnitOfWork.Context.BusinessCards
                .Where(bc => bc.UserId.Equals(userId))
                .Include(bc => bc.Company)
                .Include(bc => bc.Address)
                .ToList();
        }
        
        public BusinessCard? Get(Guid userId, Guid cardId)
        {
            return mUnitOfWork.Context.BusinessCards
                .Where(bc => bc.UserId.Equals(userId) && bc.Id.Equals(cardId))
                .Include(bc => bc.Company)
                .Include(bc => bc.Address)
                .SingleOrDefault();
        }
    }
}