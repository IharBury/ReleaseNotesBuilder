using System.Collections.Generic;

namespace ReleaseNotesBuilder.GitHub
{
    public interface IGitHubCommitProvider
    {
        /// <summary>
        /// Finds the commits.
        /// </summary>
        /// <param name="repositoryName">Name of the repository.</param>
        /// <param name="branchName">Name of the branch.</param>
        /// <param name="tagName">Name of the tag.</param>
        /// <returns></returns>
        List<CommitDataModel> FindCommits(string repositoryName, string branchName, string tagName);
    }
}