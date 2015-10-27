using RestSharp;

namespace ReleaseNotesBuilder
{
    public static class RestClientExtensions
    {
        public static IRestResponse<T> GetResponse<T>(this RestClient client, string link) where T : class, new()
        {
            var request = new RestRequest(link, Method.GET);
            var result = client.Execute<T>(request);
            return result;
        }
    }
}