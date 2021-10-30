using Contactr.Models.Authentication;
using Contactr.Persistence.Repositories.Interfaces;

namespace Contactr.Persistence.Repositories
{
    public class AuthenticationProviderRepository : Repository<AuthenticationProvider>, IAuthenticationProviderRepository
    {
        public AuthenticationProviderRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        
    }
}