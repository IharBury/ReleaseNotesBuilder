using System.Collections.Generic;

namespace ReleaseNotesBuilder.GitHub
{
    public interface IGitHubNoteCollector
    {
        void CollectNotes();
    }
}