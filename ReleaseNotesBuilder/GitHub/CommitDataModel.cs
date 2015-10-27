using RestSharp.Deserializers;

namespace ReleaseNotesBuilder.GitHub
{
    public class CommitDataModel
    {
        public string SHA { get; set; }
        [DeserializeAs(Name = "commit.message")]
        public string Message { get; set; }

    }
}