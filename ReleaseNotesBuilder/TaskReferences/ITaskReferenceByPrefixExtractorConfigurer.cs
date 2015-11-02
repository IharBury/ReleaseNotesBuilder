using System.Collections.Generic;

namespace ReleaseNotesBuilder.TaskReferences
{
    public interface ITaskReferenceByPrefixExtractorConfigurer
    {
        ICollection<string> TaskPrefixes { get; }
    }
}
