using System;

namespace Contactr.Services.GoogleServices
{
    public interface ISyncService
    {
        public void Synchronize(Guid userId);
    }
}