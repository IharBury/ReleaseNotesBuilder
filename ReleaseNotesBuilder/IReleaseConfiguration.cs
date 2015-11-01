using System.Collections.Generic;
using ReleaseNotesBuilder.GitHub;
using ReleaseNotesBuilder.Jira;

namespace ReleaseNotesBuilder
{
    public interface IReleaseConfiguration
    {
        IJiraConfiguration Jira { get; }
        IGitHubConfiguration GitHub { get; }
        string RepositoryName { get; set; }
        string BranchName { get; set; }
        string TagName { get; set; }
        ICollection<string> TaskPrefixes { get; }
    }
}