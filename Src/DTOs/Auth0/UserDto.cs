using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Contactr.DTOs.Auth0
{
    public class UserDto
    {
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("email_verified")]
        public bool EmailVerified { get; set; }

        [JsonPropertyName("given_name")]
        public string GivenName { get; set; }

        [JsonPropertyName("identities")]
        public List<IdentityDto> Identities { get; set; }

        [JsonPropertyName("locale")]
        public string Locale { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("nickname")]
        public string Nickname { get; set; }

        [JsonPropertyName("picture")]
        public string Picture { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("last_ip")]
        public string LastIp { get; set; }

        [JsonPropertyName("last_login")]
        public DateTime LastLogin { get; set; }

        [JsonPropertyName("logins_count")]
        public int LoginsCount { get; set; }
    }
}