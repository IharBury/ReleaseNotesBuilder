using System.Collections.Generic;

namespace ReleaseNotesBuilder.Formatting
{
    public interface INoteFormatter
    {
        void Format(IEnumerable<Note> notes);
    }
}