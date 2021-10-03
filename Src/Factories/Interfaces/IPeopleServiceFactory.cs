using Google.Apis.PeopleService.v1;

namespace Contactr.Factories.Interfaces
{
    public interface IPeopleServiceFactory
    {
        public PeopleServiceService Create(string refreshToken);
    }
}