using System;
using System.Threading.Tasks;

namespace Contactr.Services.GoogleServices
{
    public interface ISyncService
    {
        public Task Synchronize(Guid userId);
    }
}