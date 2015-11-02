using System;
using System.Collections.Generic;
using System.Linq;
using ReleaseNotesBuilder.Formatting;
using ReleaseNotesBuilder.Jira;
using ReleaseNotesBuilder.TaskReferences;
using RestSharp;
using RestSharp.Authenticators;

namespace ReleaseNotesBuilder.GitHub
{
    public class GitHubClient : IGitHubClient
    {
        private readonly ITaskReferenceExtractor taskReferenceExtractor;

        public GitHubClient(ITaskReferenceExtractor taskReferenceExtractor)
        {
            if (taskReferenceExtractor == null)
                throw new ArgumentNullException("taskReferenceExtractor");

            this.taskReferenceExtractor = taskReferenceExtractor;
        }

        public string OwnerName { get; set; }
        public string AccessToken { get; set; }
        public string RepositoryName { get; set; }
        public string BranchName { get; set; }
        public string TagName { get; set; }

        public void CollectNotes()
        {
            taskReferenceExtractor.Extract(FindCommits().Select(commit => commit.Message));
        }

        private List<CommitDataModel> FindCommits()
        {
            var result = new List<CommitDataModel>();
            var page = 1;
            var tag = FindTagByName();
            LinkedResponsePayload<List<CommitDataModel>> response = null;

            do
            {
                var link = string.Format("repos/{0}/{1}/commits?sha={2}&page={3}&per_page=100", OwnerName, RepositoryName,
                    BranchName, page++);

                response = GetResponse<List<CommitDataModel>>(link);

                foreach (var commit in response.Data)
                {
                    if (tag != null && commit.SHA == tag.Sha)
                    {
                        return result;
                    }
                    result.Add(commit);
                }
            }
            while (response.NextPageAvailable);

            return result;
        }

        private TagDataModel FindTagByName()
        {
            var link = string.Format("repos/{0}/{1}/git/refs/tags/{2}", OwnerName, RepositoryName, TagName);
            var response = GetResponse<TagDataModel>(link);
            var tagSHA = response.Data.Sha;
            if (!string.IsNullOrWhiteSpace(tagSHA))
            {
                link = string.Format("repos/{0}/{1}/git/tags/{2}", OwnerName, RepositoryName, tagSHA);
                response = GetResponse<TagDataModel>(link);
                return response.Data;
            }
            return null;

        }

        private LinkedResponsePayload<T> GetResponse<T>(string link) where T : class, new()
        {
            var client = new RestClient(new Uri("https://api.github.com"))
            {
                Authenticator = new OAuth2UriQueryParameterAuthenticator(AccessToken)
            };
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