using Newtonsoft.Json;

namespace TriviaGame2
{
    public class TokenResponse
    {
        [JsonProperty("response_code")]
        public int ResponseCode { get; set; }

        [JsonProperty("response_message")]
        public string ResponseMessage { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
