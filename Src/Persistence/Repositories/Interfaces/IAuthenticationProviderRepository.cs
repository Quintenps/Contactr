using Contactr.Models.Authentication;

namespace Contactr.Persistence.Repositories.Interfaces
{
    public interface IAuthenticationProviderRepository : IRepository<AuthenticationProvider>
    {
        public AuthenticationProvider? GetProviderWithUserOrDefault(string providerKey);
    }
}