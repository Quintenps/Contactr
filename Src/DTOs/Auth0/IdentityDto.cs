using Newtonsoft.Json;

namespace Contactr.DTOs.Auth0
{
    public class IdentityDto
    {
        [JsonProperty("provider")]
        public string Provider { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("connection")]
        public string Connection { get; set; }

        [JsonProperty("isSocial")]
        public bool IsSocial { get; set; }
    }
}