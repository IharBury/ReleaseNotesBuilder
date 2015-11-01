using System.Collections.Generic;

namespace ReleaseNotesBuilder
{
    public interface INoteProvider
    {
        void Collect();
    }
}