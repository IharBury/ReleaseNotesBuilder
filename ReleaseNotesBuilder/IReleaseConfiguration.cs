using System.Collections.Generic;
using ReleaseNotesBuilder.GitHub;
using ReleaseNotesBuilder.Jira;

namespace ReleaseNotesBuilder
{
    public interface IReleaseConfiguration
    {
        IJiraConfiguration Jira { get; }
        IGitHubConfiguration GitHub { get; }
        ICollection<string> TaskPrefixes { get; }
    }
}