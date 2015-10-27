using System;

using ReleaseNotesBuilder.Providers.Jira.Models;

using RestSharp;
using RestSharp.Authenticators;

namespace ReleaseNotesBuilder.Providers.Jira
{
    public class Jira
    {
        public string UserName { get; set; }
        public string Password { get; set; }


        public TaskDataModel GetTask(string taskName)
        {
            var client = new RestClient(new Uri("https://evisionindustysoftware.atlassian.net"))
            {
                Authenticator = new HttpBasicAuthenticator(UserName, Password)
            };
            var link = string.Format("rest/api/2/issue/{0}?fields=summary,customfield_11000", taskName);
            var request = new RestRequest(link, Method.GET);
            var result = client.Execute<TaskDataModel>(request);
            return result.Data;
        }
    }
}