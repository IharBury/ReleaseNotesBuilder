using System.Collections.Generic;

namespace ReleaseNotesBuilder.TaskReferences
{
    public class TaskReferenceByPrefixExtractorConfiguration : ITaskReferenceByPrefixExtractorConfigurer
    {
        public TaskReferenceByPrefixExtractorConfiguration()
        {
            TaskPrefixes = new List<string>();
        }

        public ICollection<string> TaskPrefixes { get; private set; }
    }
}
