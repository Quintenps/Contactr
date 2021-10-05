using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Contactr.Services.GoogleServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contactr.Controllers
{
    [Route("api/[controller]")]
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

        [HttpGet]
        public async Task Test()
        {
            Guid userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException());
            await _connectionService.ReadGoogleContacts(userId);
        }

        [HttpGet("2")]
        public async Task Test2()
        {
            Guid userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException());
            _syncService.Synchronize(userId);
        }
    }
}