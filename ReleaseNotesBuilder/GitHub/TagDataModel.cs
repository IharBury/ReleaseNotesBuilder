using RestSharp.Deserializers;

namespace ReleaseNotesBuilder.GitHub
{
    public class TagDataModel
    {
        [DeserializeAs(Name = "object.sha")]
        public string SHA { get; set; }
    }
}