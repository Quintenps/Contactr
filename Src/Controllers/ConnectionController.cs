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

        public ConnectionController(IConnectionService connectionService)
        {
            _connectionService = connectionService ?? throw new ArgumentNullException(nameof(connectionService));
        }

        [HttpGet]
        public async Task Test()
        {
            Guid userId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException());
            await _connectionService.ReadGoogleContacts(userId);
        }
    }
}