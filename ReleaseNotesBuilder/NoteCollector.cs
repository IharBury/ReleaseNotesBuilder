using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ReleaseNotesBuilder.GitHub;
using ReleaseNotesBuilder.Jira;

namespace ReleaseNotesBuilder
{
    public class NoteCollector : INoteCollector
    {
        private readonly GitHubClient gitHub;
        private readonly JiraClient jira;

        public NoteCollector(GitHubClient gitHub, JiraClient jira)
        {
            this.gitHub = gitHub;
            this.jira = jira;
            TaskPrefixes = new List<string>();
        }

        public string RepositoryName { get; set; }
        public string BranchName { get; set; }
        public string TagName { get; set; }
        public ICollection<string> TaskPrefixes { get; private set; }

        public List<Note> Collect()
        {
            var taskCriteria = TaskPrefixes
                .Select(x => new Regex(x + "-(\\d){1,}", RegexOptions.Multiline | RegexOptions.IgnoreCase))
                .ToArray();

            var taskNames = gitHub.GetTaskNamesByCommitDescription(RepositoryName, BranchName, TagName, taskCriteria);

            return taskNames
                .Select(taskName => new Note
                {
                    TaskName = taskName,
                    Summary = jira.GetTask(taskName).Summary
                })
                .ToList();
        }
    }
}