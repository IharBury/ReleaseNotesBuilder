using System.Collections.Generic;
using ReleaseNotesBuilder.Formatting;
using ReleaseNotesBuilder.GitHub;
using ReleaseNotesBuilder.Jira;
using ReleaseNotesBuilder.TaskReferences;

namespace ReleaseNotesBuilder
{
    public interface IProgramConfigurer
    {
        IJiraConfigurer Jira { get; }
        IGitHubConfigurer GitHub { get; }
        ITaskReferenceByPrefixExtractorConfigurer TaskReferenceExtractor { get; }
        IRazorTemplateNoteFormatterConfigurer NoteFormatter { get; }
    }
}