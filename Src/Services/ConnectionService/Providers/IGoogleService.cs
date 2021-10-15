using System;
using Contactr.Models.Authentication;
using Google.Apis.PeopleService.v1;
using static Google.Apis.PeopleService.v1.PeopleResource.ConnectionsResource;

namespace Contactr.Services.ConnectionService.Providers
{
    public interface IGoogleService
    {
        public PeopleServiceService GetPeopleService(Guid userId);
        public AuthenticationProvider GetAuthenticationProvider(Guid userId);
        public ListRequest GetListRequest(Guid userId);
    }
}