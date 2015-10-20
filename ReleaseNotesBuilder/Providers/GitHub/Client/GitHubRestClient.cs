using System;
using System.Linq;

using RestSharp;
using RestSharp.Authenticators;

namespace ReleaseNotesBuilder.Providers.GitHub.Client
{
    public class GitHubRestClient : IGitHubRestClient
    {
        private readonly RestClient client;

        public string OwnerName { get; private set; }

        public GitHubRestClient(string ownerName, string accessToken)
        {
            OwnerName = ownerName;
            client = new RestClient(new Uri("https://api.github.com"))
            {
                Authenticator = new OAuth2UriQueryParameterAuthenticator(accessToken)
            };
        }

        public LinkedResponsePayload<T> GetResponse<T>(string link) where T : class, new()
        {
            var request = new RestRequest(link, Method.GET);
            var result = client.Execute<T>(request);
            return new LinkedResponsePayload<T>
            {
                Data = result.Data,
                NextPageAvailable = NextPageAvailable(result)
            };
        }

        private static bool NextPageAvailable(IRestResponse response)
        {
            return response.Headers.FirstOrDefault(x => x.Name == "Link") != null;
        }
    }
}