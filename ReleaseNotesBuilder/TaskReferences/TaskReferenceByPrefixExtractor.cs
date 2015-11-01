using System.Collections.Generic;

namespace ReleaseNotesBuilder.TaskReferences
{
    public class TaskReferenceByPrefixExtractor : ITaskReferenceByPrefixExtractorConfigurer
    {
        public TaskReferenceByPrefixExtractor()
        {
            TaskPrefixes = new List<string>();
        }


        public ICollection<string> TaskPrefixes { get; private set; }
    }
}
