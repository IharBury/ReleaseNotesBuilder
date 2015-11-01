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
        private readonly IJiraClient jira;
        private readonly INoteFormatter noteFormatter;

        public GitHubClient(ITaskReferenceExtractor taskReferenceExtractor, IJiraClient jira, INoteFormatter noteFormatter)
        {
            if (taskReferenceExtractor == null)
                throw new ArgumentNullException("taskReferenceExtractor");
            if (jira == null)
                throw new ArgumentNullException("jira");
            if (noteFormatter == null)
                throw new ArgumentNullException("noteFormatter");

            this.taskReferenceExtractor = taskReferenceExtractor;
            this.jira = jira;
            this.noteFormatter = noteFormatter;
        }


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
        /// <returns></returns>
        public List<string> GetTaskNamesByCommitDescription()
        {
            return taskReferenceExtractor.Extract(FindCommits().Select(commit => commit.Message)).ToList();
        }

        public void CollectNotes()
        {
            var taskNames = GetTaskNamesByCommitDescription();

            var notes = taskNames
                .Select(taskName => new Note
                {
                    TaskName = taskName,
                    Summary = jira.GetTask(taskName).Summary
                })
                .ToList();
            noteFormatter.Format(notes);
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