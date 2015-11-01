using System.Collections.Generic;

namespace ReleaseNotesBuilder.Formatting
{
    public interface INoteFormatter
    {
        string Format(IEnumerable<Note> notes);
    }
}