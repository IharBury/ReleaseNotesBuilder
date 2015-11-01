using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ReleaseNotesBuilder.TaskReferences
{
    public class TaskReferenceByPrefixExtractor : ITaskReferenceByPrefixExtractorConfigurer, ITaskReferenceExtractor
    {
        public TaskReferenceByPrefixExtractor()
        {
            TaskPrefixes = new List<string>();
        }


        public ICollection<string> TaskPrefixes { get; private set; }


        public IEnumerable<string> Extract(IEnumerable<string> commitMessages)
        {
            var taskNameCriteria = TaskPrefixes
                .Select(x => new Regex(x + "-(\\d){1,}", RegexOptions.Multiline | RegexOptions.IgnoreCase))
                .ToList();
            return (
                from commitMessage in commitMessages
                from criteria in taskNameCriteria
                where commitMessage != null
                from Match match in criteria.Matches(commitMessage)
                select match.Value.Trim())        
                    .Distinct();
        }
    }
}
