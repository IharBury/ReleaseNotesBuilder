using System.Collections.Generic;

using RestSharp.Deserializers;

namespace ReleaseNotesBuilder.Providers.Jira.Models
{
    public class TaskDataModel
    {
        [DeserializeAs(Name = "fields.customfield_11000")]
        public List<string> Requirements { get; set; }
        [DeserializeAs(Name = "fields.summary")]
        public string Summary { get; set; }
    }
}