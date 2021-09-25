using Contactr.Models;
using Contactr.Persistence.Repositories.Interfaces;

namespace Contactr.Persistence.Repositories
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        public CompanyRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}