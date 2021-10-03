using System;
using System.Threading.Tasks;

namespace Contactr.Services.GoogleServices
{
    public interface IConnectionService
    {
        public Task ReadGoogleContacts(Guid userId);
    }
}