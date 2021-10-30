using System;
using System.Threading.Tasks;
using Contactr.DTOs.Cards;
using Contactr.Models;

namespace Contactr.Services.AuthService
{
    public interface IAuthService
    {
        public Task<User> CreateUser(Guid uuid, string auth0Id);
    }
}