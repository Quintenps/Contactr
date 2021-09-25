using Contactr.Models;

namespace Contactr.Factories.Interfaces
{
    public interface IUserFactory
    {
        public User Create(string email, string? avatar);
    }
}