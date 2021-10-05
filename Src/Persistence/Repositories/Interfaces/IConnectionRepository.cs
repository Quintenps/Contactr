using System;
using System.Collections.Generic;
using Contactr.Models.Connection;

namespace Contactr.Persistence.Repositories.Interfaces
{
    public interface IConnectionRepository : IRepository<Connection>
    {
        public IEnumerable<Connection> GetConnections(Guid userId);
    }
}