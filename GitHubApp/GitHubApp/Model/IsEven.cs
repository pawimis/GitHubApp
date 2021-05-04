using Newtonsoft.Json;

namespace GitHubApp.Model
{
    public class IsEven
    {
        [JsonProperty("iseven")]
        public bool Iseven { get; set; }

        [JsonProperty("ad")]
        public string Ad { get; set; }
    }
}
