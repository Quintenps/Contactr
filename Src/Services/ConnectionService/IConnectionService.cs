using System;
using System.Threading.Tasks;

namespace Contactr.Services.ConnectionService
{
    public interface IConnectionService
    {
        public Task ReadGoogleContacts(Guid userId);
    }
}