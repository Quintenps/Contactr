using Google.Apis.PeopleService.v1;
using Google.Apis.PeopleService.v1.Data;

namespace Contactr.Factories.Interfaces
{
    public interface IPeopleServiceFactory
    {
        public PeopleServiceService CreatePeopleServiceClient(string refreshToken);
        public Name CreateName(string firstName, string lastName, string fullName);
        public Organization CreateOrganization(string companyName, string jobTitle);
        public EmailAddress CreateEmail(string type, string emailAddress);
        public Birthday CreateBirthday(Date birthday);
        public Address CreateAddress(string type, string city, string country, string postalcode, string street);
        public UpdateContactPhotoRequest CreatePhoto();
    }
}