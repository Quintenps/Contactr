using System;
using System.Threading.Tasks;

namespace Contactr.Services.ConnectionService
{
    public interface ISyncService
    {
        public Task Synchronize(Guid userId);
    }
}