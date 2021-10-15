using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Contactr.Services.ConnectionService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contactr.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class ConnectionController : ControllerBase
    {
        private readonly IConnectionService _connectionService;
        private readonly ISyncService _syncService;

        public ConnectionController(IConnectionService connectionService, ISyncService syncService)
        {
            _connectionService = connectionService ?? throw new ArgumentNullException(nameof(connectionService));
            _syncService = syncService ?? throw new ArgumentNullException(nameof(syncService));
        }

        [HttpGet("create")]
        public async Task CreateConnections()
        {
            Guid userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException());
            await _connectionService.ReadGoogleContacts(userId);
        }

        [HttpGet("synchronize")]
        public async Task SynchronizeConnections()
        {
            Guid userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException());
            await _syncService.Synchronize(userId);
        }
    }
}