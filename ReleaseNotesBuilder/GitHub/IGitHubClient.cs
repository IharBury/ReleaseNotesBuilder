using System.Collections.Generic;

namespace ReleaseNotesBuilder.GitHub
{
    public interface IGitHubClient
    {
        /// <summary>
        /// Finds the commits.
        /// </summary>
        List<CommitDataModel> FindCommits();

        List<string> GetTaskNamesByCommitDescription();
    }
}