using System;
using Contactr.Factories.Interfaces;
using Contactr.Models;

namespace Contactr.Factories
{
    public class UserFactory : IUserFactory
    {
        
        public User Create(string email, string? avatar)
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Email = email,
                Avatar = avatar
            };
            
            return user;
        }
    }
}