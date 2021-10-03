using System;
using System.Threading.Tasks;
using Contactr.DTOs.AuthenticationProvider;
using Contactr.Services.AuthService;
using Microsoft.AspNetCore.Mvc;

namespace Contactr.Controllers
{
    [Route("api/[controller]")]
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

        [HttpPost("login")]
        public async Task<ActionResult> Login(GoogleLoginDto googleLoginDto)
        {
            var googlePayload = await _authService.VerifyGoogleToken(googleLoginDto);
            var jwt = await _authService.Login(googlePayload, googleLoginDto.RefreshToken);
            return Ok(jwt);
        }
    }
}