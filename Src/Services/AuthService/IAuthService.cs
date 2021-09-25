using System.Threading.Tasks;
using Contactr.DTOs.AuthenticationProvider;
using Google.Apis.Auth;

namespace Contactr.Services.AuthService
{
    public interface IAuthService
    {
        public Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(GoogleLoginDto externalAuth);
        public Task<string> Login(GoogleJsonWebSignature.Payload payload);
    }
}