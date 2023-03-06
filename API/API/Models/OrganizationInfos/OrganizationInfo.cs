using API.Models.OrganizationInfos.ValueObjects;
using System.Text.Json.Serialization;

namespace API.Models.OrganizationInfos
{
    public class OrganizationInfo
    {
        [JsonPropertyName("organisasjonsnummer")]
        public string OrgNo { get; set; }
        public string Name { get; set; }
        [JsonPropertyName("navn")]
        public string BrregNavn { get; set; }
        [JsonPropertyName("antallAnsatte")]
        public int AntallAnsatte { get; set; }
        [JsonPropertyName("naeringskode1")]
        public Naeringskode Naeringskode1 { get; set; } = new Naeringskode();
        [JsonPropertyName("organisasjonsform")]
        public Organisasjonsform Organisasjonsform { get; set; } = new Organisasjonsform();
        [JsonPropertyName("konkurs")]
        public bool Konkurs { get; set; }
        public StatusEnum Status { get; set; }
    }
}
