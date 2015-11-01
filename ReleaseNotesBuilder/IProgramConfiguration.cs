using System.Collections.Generic;
using ReleaseNotesBuilder.Formatting;
using ReleaseNotesBuilder.GitHub;
using ReleaseNotesBuilder.Jira;

namespace ReleaseNotesBuilder
{
    public interface IProgramConfiguration
    {
        IJiraConfiguration Jira { get; }
        IGitHubConfiguration GitHub { get; }
        ICollection<string> TaskPrefixes { get; }
        string TemplateName { get; set; }
    }
}