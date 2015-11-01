using System;
using System.Linq;
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

        public NoteCollector(
            IGitHubClient gitHub, 
            IJiraClient jira, 
            INoteFormatter noteFormatter)
        {
            if (gitHub == null)
                throw new ArgumentNullException("gitHub");
            if (jira == null)
                throw new ArgumentNullException("jira");
            if (noteFormatter == null)
                throw new ArgumentNullException("noteFormatter");

            this.gitHub = gitHub;
            this.jira = jira;
            this.noteFormatter = noteFormatter;
        }

        public void Collect()
        {
            var taskNames = gitHub.GetTaskNamesByCommitDescription();

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