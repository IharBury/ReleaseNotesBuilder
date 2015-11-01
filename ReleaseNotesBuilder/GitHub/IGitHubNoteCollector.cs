using System.Collections.Generic;

namespace ReleaseNotesBuilder.GitHub
{
    public interface IGitHubNoteCollector
    {
        /// <summary>
        /// Finds the commits.
        /// </summary>
        List<CommitDataModel> FindCommits();

        List<string> GetTaskNamesByCommitDescription();

        void CollectNotes();
    }
}