using System.Collections.Generic;

namespace ReleaseNotesBuilder.Builders.Notes
{
    public interface INotesBuilder
    {
        List<string> Build();
    }
}