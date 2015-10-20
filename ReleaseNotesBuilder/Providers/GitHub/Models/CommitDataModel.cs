using RestSharp.Deserializers;

namespace ReleaseNotesBuilder.Providers.GitHub.Models
{
    public class CommitDataModel
    {
        public string SHA { get; set; }
        [DeserializeAs(Name = "commit.message")]
        public string Message { get; set; }

    }
}