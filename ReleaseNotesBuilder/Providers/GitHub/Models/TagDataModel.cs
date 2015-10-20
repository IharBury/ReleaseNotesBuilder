using RestSharp.Deserializers;

namespace ReleaseNotesBuilder.Providers.GitHub.Models
{
    public class TagDataModel
    {
        [DeserializeAs(Name = "object.sha")]
        public string SHA { get; set; }
    }
}