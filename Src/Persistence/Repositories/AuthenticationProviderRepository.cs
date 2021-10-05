using System.Linq;
using Contactr.Models.Authentication;
using Contactr.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Contactr.Persistence.Repositories
{
    public class AuthenticationProviderRepository : Repository<AuthenticationProvider>, IAuthenticationProviderRepository
    {
        public AuthenticationProviderRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public AuthenticationProvider? GetProviderWithUserOrDefault(string providerKey)
        {
            return MUnitOfWork.Context.AuthenticationProviders
                .Where(ap => ap.Key.Equals(providerKey))
                .Include(ap => ap.User)
                .FirstOrDefault();
        }
    }
}