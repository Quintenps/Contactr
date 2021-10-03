using System;
using System.Linq;
using Contactr.Models.Cards;
using Contactr.Persistence.Repositories.Interfaces;

namespace Contactr.Persistence.Repositories
{
    public class PersonalCardRepository : Repository<PersonalCard>, IPersonalCardRepository
    {
        public PersonalCardRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}