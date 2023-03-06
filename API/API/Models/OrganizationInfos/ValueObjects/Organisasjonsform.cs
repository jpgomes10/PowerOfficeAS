using System.Text.Json.Serialization;

namespace API.Models.OrganizationInfos.ValueObjects
{
    public class Organisasjonsform
    {
        [JsonPropertyName("kode")]
        public string Kode { get; set; }
    }
}
