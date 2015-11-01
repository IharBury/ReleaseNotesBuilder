using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using RestSharp;
using RestSharp.Authenticators;

namespace ReleaseNotesBuilder.GitHub
{
    public class GitHubClient : IGitHubClient
    {
        public string OwnerName { get; set; }
        public string AccessToken { get; set; }
        public string RepositoryName { get; set; }
        public string BranchName { get; set; }
        public string TagName { get; set; }


        /// <summary>
        /// Finds the commits.
        /// </summary>
        public List<CommitDataModel> FindCommits()
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

        /// <summary>
        /// Finds the name of the tag by.
        /// </summary>
        public TagDataModel FindTagByName()
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

        /// <summary>
        ///     Gets the task names by commit description.
        /// </summary>
        /// <param name="taskNameCriteria">The task name criteria.</param>
        /// <returns></returns>
        public List<string> GetTaskNamesByCommitDescription(Regex[] taskNameCriteria)
        {
            var commits = FindCommits();

            var taskNames = (from criteria in taskNameCriteria
                from commit in commits
                where commit.Message != null
                from Match match in criteria.Matches(commit.Message)
                select match.Value.Trim())
                .Distinct()
                .OrderBy(x => x)
                .ToList();
            return taskNames;
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