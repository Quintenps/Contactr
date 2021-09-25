using System;
using System.Threading.Tasks;
using AutoMapper;
using Contactr.Persistence.Repositories;
using Contactr.Persistence.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace Contactr.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        public AppDbContext Context { get; set; }
        private ILogger<UnitOfWork> Logger { get; }
        public IUserRepository UserRepository { get; }
        public IAddressRepository AddressRepository { get; }
        public ICompanyRepository CompanyRepository { get; }
        public IConnectionRepository ConnectionRepository { get; }
        public IPersonalCardRepository PersonalCardRepository { get; }
        public IBusinessCardRepository BusinessCardRepository { get; }
        public IAuthenticationProviderRepository AuthenticationProviderRepository { get; }

        public UnitOfWork(AppDbContext context, IMapper mapper, ILogger<UnitOfWork> logger)
        {
            Context = context;
            Logger = logger;

            UserRepository = new UserRepository(this);
            AddressRepository = new AddressRepository(this);
            CompanyRepository = new CompanyRepository(this);
            ConnectionRepository = new ConnectionRepository(this);
            PersonalCardRepository = new PersonalCardRepository(this);
            BusinessCardRepository = new BusinessCardRepository(this);
            AuthenticationProviderRepository = new AuthenticationProviderRepository(this);
        }

        public void Dispose()
        {
            if (Context != null)
            {
                Context.Dispose();
                Context = null;
            }
        }


        public async Task Save()
        {
            try
            {
              await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogWarning($"Error - Couldn't save changes {ex.Message}");
            }
        }
    }
}