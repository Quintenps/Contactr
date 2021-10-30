using System;
using System.Threading.Tasks;
using Contactr.DTOs.Cards;
using Contactr.Models;
using Contactr.Services.AuthService;
using Microsoft.AspNetCore.Mvc;

namespace Contactr.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        [HttpGet("status")]
        public ActionResult Status()
        {
            return Ok();
        }

        // Todo: Protect API
        [HttpPost("{uuid:guid}/{auth0Id}")]
        public async Task<ActionResult<User>> CreateAccount(Guid uuid, string auth0Id)
        {
            var user = await _authService.CreateUser(uuid, auth0Id);
            return Created("Created", user);
        }
    }
}