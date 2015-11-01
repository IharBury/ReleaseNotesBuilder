using System.Collections.Generic;

namespace ReleaseNotesBuilder.GitHub
{
    public interface IGitHubCommitProvider
    {
        /// <summary>
        /// Finds the commits.
        /// </summary>
        List<CommitDataModel> FindCommits();
    }
}