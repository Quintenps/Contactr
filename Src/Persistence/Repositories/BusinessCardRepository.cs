using Contactr.Models.Cards;
using Contactr.Persistence.Repositories.Interfaces;

namespace Contactr.Persistence.Repositories
{
    public class BusinessCardRepository : Repository<BusinessCard>, IBusinessCardRepository
    {
        public BusinessCardRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}