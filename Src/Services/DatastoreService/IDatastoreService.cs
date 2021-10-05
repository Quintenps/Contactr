using System;
using Google.Apis.PeopleService.v1.Data;

namespace Contactr.Services.DatastoreService
{
    public interface IDatastoreService
    {
        public void MakeRequest(Guid userId, Person person);
    }
}