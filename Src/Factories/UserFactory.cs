using System;
using Contactr.Factories.Interfaces;
using Contactr.Models;

namespace Contactr.Factories
{
    public class UserFactory : IUserFactory
    {
        public User Create(Guid uuid, string auth0Id)
        {
            var user = new User
            {
                Id = uuid,
                Auth0Id = auth0Id
            };
            
            return user;
        }
    }
}