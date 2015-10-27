using System.Collections.Generic;

namespace ReleaseNotesBuilder
{
    public interface INoteCollector
    {
        List<Note> Collect();
    }
}