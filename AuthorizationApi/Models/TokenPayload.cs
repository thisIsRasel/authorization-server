using Newtonsoft.Json;

namespace AuthorizationApi.Models
{
    public class TokenPayload
    {
        [JsonProperty("grant_type")]
        public string GrantType { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
