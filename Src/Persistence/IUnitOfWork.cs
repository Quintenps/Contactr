using System.Threading.Tasks;
using Contactr.Persistence.Repositories.Interfaces;

namespace Contactr.Persistence
{
    public interface IUnitOfWork
    {
        public AppDbContext Context { get; set; }
        public IUserRepository UserRepository { get; }
        public IAddressRepository AddressRepository { get; }
        public ICompanyRepository CompanyRepository { get; }
        public IConnectionRepository ConnectionRepository { get; }
        public IPersonalCardRepository PersonalCardRepository { get; }
        public IBusinessCardRepository BusinessCardRepository { get; }
        public IAuthenticationProviderRepository AuthenticationProviderRepository { get; }
        Task Save();
    }
}