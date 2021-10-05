using System;
using Contactr.Factories.Interfaces;
using Contactr.Models.Connection;
using Contactr.Models.Connection.Resources;
using Contactr.Models.Enums;

namespace Contactr.Factories
{
    public class ConnectionFactory : IConnectionFactory
    {
        private Resource CreateGoogleResource(string eTag, string resourceName)
        {
            return new Contactr.Models.Connection.Resources.Google
            {
                ETag = eTag,
                ResourceName = resourceName,
                Provider = SyncProviders.Google
            };
        }
        
        public Connection Create(Guid receiverUserId, Guid senderUserId, string eTag, string resourceName)
        {
            return new Connection()
            {
                ReceiverUserId = receiverUserId,
                SenderUserId = senderUserId,
                Resource = CreateGoogleResource(eTag, resourceName),
                CreatedAt = DateTime.Now,
                SynchronizedAt = null
            };
        }
    }
}