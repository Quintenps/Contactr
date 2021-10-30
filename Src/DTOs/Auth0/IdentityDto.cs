using System.Text.Json.Serialization;

namespace Contactr.DTOs.Auth0
{
    public class IdentityDto
    {
        [JsonPropertyName("provider")]
        public string Provider { get; set; }

        [JsonPropertyName("AccessToken")]
        public string AccessToken { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("connection")]
        public string Connection { get; set; }

        [JsonPropertyName("isSocial")]
        public bool IsSocial { get; set; }
    }
}