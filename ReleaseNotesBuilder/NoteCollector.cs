using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ReleaseNotesBuilder.Formatting;
using ReleaseNotesBuilder.GitHub;
using ReleaseNotesBuilder.Jira;

namespace ReleaseNotesBuilder
{
    public class NoteCollector : INoteCollector
    {
        private readonly IGitHubClient gitHub;
        private readonly IJiraClient jira;
        private readonly INoteFormatter noteFormatter;

        public NoteCollector(IGitHubClient gitHub, IJiraClient jira, INoteFormatter noteFormatter)
        {
            this.gitHub = gitHub;
            this.jira = jira;
            this.noteFormatter = noteFormatter;
            TaskPrefixes = new List<string>();
        }

        public ICollection<string> TaskPrefixes { get; private set; }

        public IJiraConfiguration Jira
        {
            get { return jira; }
        }

        public IGitHubConfiguration GitHub
        {
            get { return gitHub; }
        }

        public void Collect()
        {
            var taskCriteria = TaskPrefixes
                .Select(x => new Regex(x + "-(\\d){1,}", RegexOptions.Multiline | RegexOptions.IgnoreCase))
                .ToArray();

            var taskNames = gitHub.GetTaskNamesByCommitDescription(taskCriteria);

            var notes = taskNames
                .Select(taskName => new Note
                {
                    TaskName = taskName,
                    Summary = jira.GetTask(taskName).Summary
                })
                .ToList();
            noteFormatter.Format(notes);
        }
    }
}