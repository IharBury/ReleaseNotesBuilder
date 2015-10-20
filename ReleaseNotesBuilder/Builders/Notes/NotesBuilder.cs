using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using ReleaseNotesBuilder.Providers.GitHub;
using ReleaseNotesBuilder.Providers.GitHub.Client;
using ReleaseNotesBuilder.Providers.Jira;
using ReleaseNotesBuilder.Providers.Jira.Client;

namespace ReleaseNotesBuilder.Builders.Notes
{
    public class NotesBuilder: INotesBuilder
    {
        private readonly NotesBuilderConfiguration notesBuilderConfiguration;

        public NotesBuilder(NotesBuilderConfiguration notesBuilderConfiguration)
        {
            this.notesBuilderConfiguration = notesBuilderConfiguration;
        }

        public List<string> Build(string repositoryName, string branchName, string tagName, string[] taskPrefixes)
        {
            var gitHubClient = new GitHubRestClient(notesBuilderConfiguration.GithubOwnerName, notesBuilderConfiguration.GithubAccessToken);
            var gitHubPovider = new GitHubDataProvider(gitHubClient);

            var jiraClient = new JiraRestClient(notesBuilderConfiguration.JiraUsername, notesBuilderConfiguration.JiraPassword);
            var jiraProvider = new JiraDataProvider(jiraClient);

            var taskCriteria = taskPrefixes
                .Select(x => new Regex(x + "-(\\d){1,}", RegexOptions.Multiline | RegexOptions.IgnoreCase))
                .ToArray();

            var taskNames = gitHubPovider.GetTaskNamesByCommitDescription(repositoryName, branchName, tagName, taskCriteria);

            return taskNames.Select(taskName => string.Format("{0} {1}", taskName, jiraProvider.GetTask(taskName).Summary)).ToList();
        }
    }
}