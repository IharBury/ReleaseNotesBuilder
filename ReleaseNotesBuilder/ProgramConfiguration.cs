using System;
using ReleaseNotesBuilder.Formatting;
using ReleaseNotesBuilder.GitHub;
using ReleaseNotesBuilder.Jira;
using ReleaseNotesBuilder.TaskReferences;

namespace ReleaseNotesBuilder
{
    public class ProgramConfiguration : IProgramConfigurer
    {
        public ProgramConfiguration(
            IJiraConfigurer jira, 
            IGitHubConfigurer gitHub, 
            ITaskReferenceByPrefixExtractorConfigurer taskReferenceExtractor, 
            IRazorTemplateNoteFormatterConfigurer noteFormatter)
        {
            if (jira == null)
                throw new ArgumentNullException("jira");
            if (gitHub == null)
                throw new ArgumentNullException("gitHub");
            if (taskReferenceExtractor == null)
                throw new ArgumentNullException("taskReferenceExtractor");
            if (noteFormatter == null)
                throw new ArgumentNullException("noteFormatter");

            Jira = jira;
            GitHub = gitHub;
            TaskReferenceExtractor = taskReferenceExtractor;
            NoteFormatter = noteFormatter;
        }

        public IJiraConfigurer Jira { get; private set; }
        public IGitHubConfigurer GitHub { get; private set; }
        public ITaskReferenceByPrefixExtractorConfigurer TaskReferenceExtractor { get; private set; }
        public IRazorTemplateNoteFormatterConfigurer NoteFormatter { get; private set; }
    }
}
