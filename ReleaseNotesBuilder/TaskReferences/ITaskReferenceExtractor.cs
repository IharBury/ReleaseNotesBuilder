using System.Collections.Generic;

namespace ReleaseNotesBuilder.TaskReferences
{
    public interface ITaskReferenceExtractor
    {
        void Extract(IEnumerable<string> commitMessages);
    }
}