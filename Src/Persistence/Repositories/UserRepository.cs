using Contactr.Models;
using Contactr.Persistence.Repositories.Interfaces;

namespace Contactr.Persistence.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}