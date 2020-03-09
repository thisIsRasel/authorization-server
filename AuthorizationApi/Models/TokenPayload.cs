using Newtonsoft.Json;

namespace AuthorizationApi.Models
{
    public class TokenPayload
    {
        [JsonProperty("grant_type")]
        public string GrantType { get; set; }
        
        [JsonProperty("username")]
        public string Username { get; set; }
        
        [JsonProperty("password")]
        public string Password { get; set; }
        
        [JsonProperty("client_id")]
        public string ClientId { get; set; }
        
        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
