using Contactr.Models.Connection;
using Contactr.Persistence.Repositories.Interfaces;

namespace Contactr.Persistence.Repositories
{
    public class ConnectionRepository : Repository<Connection>, IConnectionRepository
    {
        public ConnectionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}