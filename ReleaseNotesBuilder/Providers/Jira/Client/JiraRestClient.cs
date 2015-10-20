using System;

using RestSharp;
using RestSharp.Authenticators;

namespace ReleaseNotesBuilder.Providers.Jira.Client
{
    public class JiraRestClient: IJiraRestClient
    {
        private readonly RestClient client;

        public JiraRestClient(string userName, string password)
        {
            client = new RestClient(new Uri("https://evisionindustysoftware.atlassian.net"))
            {
                Authenticator = new HttpBasicAuthenticator(userName, password)
            };
        }

        public T GetResponse<T>(string link) where T : class, new()
        {
            var request = new RestRequest(link, Method.GET);
            var result = client.Execute<T>(request);
            return result.Data;
        }
    }
}