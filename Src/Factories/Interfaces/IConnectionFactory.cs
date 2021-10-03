using System;
using Contactr.Models.Connection;
using Contactr.Models.Connection.Resources;

namespace Contactr.Factories.Interfaces
{
    public interface IConnectionFactory
    {
        public Connection Create(Guid receiverUserId, Guid senderUserId, string ETag);
    }
}