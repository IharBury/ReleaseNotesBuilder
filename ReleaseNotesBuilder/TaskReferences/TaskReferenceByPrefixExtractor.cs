using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ReleaseNotesBuilder.Formatting;
using ReleaseNotesBuilder.Jira;

namespace ReleaseNotesBuilder.TaskReferences
{
    public class TaskReferenceByPrefixExtractor : ITaskReferenceByPrefixExtractorConfigurer, ITaskReferenceExtractor
    {
        private readonly IJiraTaskNoteProvider jira;
        private readonly INoteFormatter noteFormatter;

        public TaskReferenceByPrefixExtractor(IJiraTaskNoteProvider jira, INoteFormatter noteFormatter)
        {
            if (jira == null)
                throw new ArgumentNullException("jira");
            if (noteFormatter == null)
                throw new ArgumentNullException("noteFormatter");

            this.jira = jira;
            this.noteFormatter = noteFormatter;
            TaskPrefixes = new List<string>();
        }


        public ICollection<string> TaskPrefixes { get; private set; }


        public void Extract(IEnumerable<string> commitMessages)
        {
            var taskNameCriteria = TaskPrefixes
                .Select(x => new Regex(x + "-(\\d){1,}", RegexOptions.Multiline | RegexOptions.IgnoreCase))
                .ToList();
            var taskReferences = (
                from commitMessage in commitMessages
                from criteria in taskNameCriteria
                where commitMessage != null
                from Match match in criteria.Matches(commitMessage)
                select match.Value.Trim())
                    .Distinct()
                    .OrderBy(taskReference => taskReference);
            var notes = taskReferences
                .Select(taskReference => new Note
                {
                    TaskName = taskReference,
                    Summary = jira.GetTask(taskReference).Summary
                })
                .ToList();
            noteFormatter.Format(notes);
        }
    }
}
