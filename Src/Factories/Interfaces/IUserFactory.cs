using System;
using Contactr.Models;

namespace Contactr.Factories.Interfaces
{
    public interface IUserFactory
    {
        public User Create(Guid uuid, string auth0Id);
    }
}