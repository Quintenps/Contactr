using System;
using Contactr.Factories.Interfaces;
using Contactr.Models.Connection;
using Contactr.Models.Connection.Resources;
using Contactr.Models.Enums;

namespace Contactr.Factories
{
    public class ConnectionFactory : IConnectionFactory
    {
        private Resource CreateGoogleResource(string ETag)
        {
            return new Contactr.Models.Connection.Resources.Google
            {
                ETag = ETag,
                Provider = SyncProviders.Google
            };
        }
        
        public Connection Create(Guid receiverUserId, Guid senderUserId, string ETag)
        {
            return new Connection()
            {
                ReceiverUserId = receiverUserId,
                SenderUserId = senderUserId,
                Resource = CreateGoogleResource(ETag),
                CreatedAt = DateTime.Now,
                SynchronizedAt = null
            };
        }
    }
}