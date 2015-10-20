using ReleaseNotesBuilder.Providers.Jira.Client;
using ReleaseNotesBuilder.Providers.Jira.Models;

namespace ReleaseNotesBuilder.Providers.Jira
{
    public class JiraDataProvider
    {
        private readonly IJiraRestClient client;

        public JiraDataProvider(IJiraRestClient client)
        {
            this.client = client;
        }

        public TaskDataModel GetTask(string taskName)
        {
            var link = string.Format("rest/api/2/issue/{0}?fields=summary,customfield_11000", taskName);
            var response = client.GetResponse<TaskDataModel>(link);
            return response;
        }
    }
}