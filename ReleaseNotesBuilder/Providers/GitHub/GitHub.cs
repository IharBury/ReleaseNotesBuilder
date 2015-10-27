using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using ReleaseNotesBuilder.Providers.GitHub.Models;

using RestSharp;
using RestSharp.Authenticators;

namespace ReleaseNotesBuilder.Providers.GitHub
{
    public class GitHub
    {
        public string OwnerName { get; set; }
        public string AccessToken { get; set; }


        /// <summary>
        /// Finds the commits.
        /// </summary>
        /// <param name="repositoryName">Name of the repository.</param>
        /// <param name="branchName">Name of the branch.</param>
        /// <param name="tagName">Name of the tag.</param>
        /// <returns></returns>
        public List<CommitDataModel> FindCommits(string repositoryName, string branchName, string tagName)
        {
            var result = new List<CommitDataModel>();
            var page = 1;
            var tag = FindTagByName(repositoryName, tagName);
            LinkedResponsePayload<List<CommitDataModel>> response = null;

            do
            {
                var link = string.Format("repos/{0}/{1}/commits?sha={2}&page={3}&per_page=100", OwnerName, repositoryName,
                    branchName, page++);

                response = GetResponse<List<CommitDataModel>>(link);

                foreach (var commit in response.Data)
                {
                    if (tag != null && commit.SHA == tag.SHA)
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
        /// <param name="repositoryName">Name of the repository.</param>
        /// <param name="tagName">Name of the tag.</param>
        /// <returns></returns>
        public TagDataModel FindTagByName(string repositoryName, string tagName)
        {
            var link = string.Format("repos/{0}/{1}/git/refs/tags/{2}", OwnerName, repositoryName, tagName);
            var response = GetResponse<TagDataModel>(link);
            var tagSHA = response.Data.SHA;
            if (!string.IsNullOrWhiteSpace(tagSHA))
            {
                link = string.Format("repos/{0}/{1}/git/tags/{2}", OwnerName, repositoryName, tagSHA);
                response = GetResponse<TagDataModel>(link);
                return response.Data;
            }
            return null;

        }

        /// <summary>
        ///     Gets the task names by commit description.
        /// </summary>
        /// <param name="repositoryName">Name of the repository.</param>
        /// <param name="branchName">Name of the branch.</param>
        /// <param name="tagName">Name of the tag.</param>
        /// <param name="taskNameCriteria">The task name criteria.</param>
        /// <returns></returns>
        public List<string> GetTaskNamesByCommitDescription(string repositoryName, string branchName, string tagName,
            Regex[] taskNameCriteria)
        {
            var commits = FindCommits(repositoryName, branchName, tagName).Where(c => c.Message != null);

            var taskNames = (from criteria in taskNameCriteria
                from commit in commits
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