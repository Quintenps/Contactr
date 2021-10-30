using System.Text.Json.Serialization;

namespace Contactr.DTOs.Auth0
{
    public class Token
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }
    }
}