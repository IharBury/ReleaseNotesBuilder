using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using ReleaseNotesBuilder.Providers.GitHub;
using ReleaseNotesBuilder.Providers.Jira;

namespace ReleaseNotesBuilder.Builders.Notes
{
    public class NotesBuilder: INotesBuilder
    {
        private readonly GitHub gitHub;
        private readonly Jira jira;

        public NotesBuilder(GitHub gitHub, Jira jira)
        {
            this.gitHub = gitHub;
            this.jira = jira;
        }

        public string RepositoryName { get; set; }
        public string BranchName { get; set; }
        public string TagName { get; set; }
        public string[] TaskPrefixes { get; set; }

        public List<string> Build()
        {
            var taskCriteria = TaskPrefixes
                .Select(x => new Regex(x + "-(\\d){1,}", RegexOptions.Multiline | RegexOptions.IgnoreCase))
                .ToArray();

            var taskNames = gitHub.GetTaskNamesByCommitDescription(RepositoryName, BranchName, TagName, taskCriteria);

            return taskNames.Select(taskName => string.Format("{0} {1}", taskName, jira.GetTask(taskName).Summary)).ToList();
        }
    }
}