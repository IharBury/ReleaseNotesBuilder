using System.Collections.Generic;

namespace ReleaseNotesBuilder
{
    public interface INoteProvider
    {
        List<Note> Collect();
    }
}