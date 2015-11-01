using System.Collections.Generic;

namespace ReleaseNotesBuilder.TaskReferences
{
    public interface ITaskReferenceExtractor
    {
        IEnumerable<string> Extract(IEnumerable<string> commitMessages);
    }
}