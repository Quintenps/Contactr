using Contactr.Models;
using Contactr.Persistence.Repositories.Interfaces;

namespace Contactr.Persistence.Repositories
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        public AddressRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}