using System;
using System.Collections.Generic;
using System.Linq;
using Contactr.Models.Connection;
using Contactr.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Contactr.Persistence.Repositories
{
    public class ConnectionRepository : Repository<Connection>, IConnectionRepository
    {
        public ConnectionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        
        public IEnumerable<Connection> GetConnections(Guid userId)
        {
            return MUnitOfWork.Context.Connections
                .Where(c => c.ReceiverUserId.Equals(userId))
                .Include(c => c.Resource)
                .Include(c => c.SenderUser)
                .ThenInclude(su => su.PersonalCard)
                .Include(su => su.SenderUser)
                .ThenInclude(su => su.BusinessCards);
        }
    }
}