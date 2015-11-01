using System.Collections.Generic;

namespace ReleaseNotesBuilder.Jira
{
    public interface IJiraTaskNoteCollector
    {
        void CollectTaskNotes(IEnumerable<string> taskReferences);
    }
}