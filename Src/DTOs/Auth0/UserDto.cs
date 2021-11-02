using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Contactr.DTOs.Auth0
{
    public class UserDto
    {
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("email_verified")]
        public bool EmailVerified { get; set; }

        [JsonProperty("given_name")]
        public string GivenName { get; set; }

        [JsonProperty("identities")]
        public List<IdentityDto> Identities { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        [JsonProperty("picture")]
        public string Picture { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("last_ip")]
        public string LastIp { get; set; }

        [JsonProperty("last_login")]
        public DateTime LastLogin { get; set; }

        [JsonProperty("logins_count")]
        public int LoginsCount { get; set; }
    }
}