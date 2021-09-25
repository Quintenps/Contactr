using System.Linq;
using System.Threading.Tasks;
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
            return mUnitOfWork.Context.AuthenticationProviders
                .Where(ap => ap.Key.Equals(providerKey))
                .Include(ap => ap.User)
                .FirstOrDefault();
        }
    }
}