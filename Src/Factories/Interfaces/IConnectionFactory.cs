using System;
using Contactr.Models.Connection;

namespace Contactr.Factories.Interfaces
{
    public interface IConnectionFactory
    {
        public Connection Create(Guid receiverUserId, Guid senderUserId, string eTag, string resourceName);
    }
}