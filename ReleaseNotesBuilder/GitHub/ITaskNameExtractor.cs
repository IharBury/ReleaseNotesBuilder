using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ReleaseNotesBuilder.GitHub
{
    public interface ITaskNameExtractor
    {
        /// <summary>
        ///     Gets the task names by commit description.
        /// </summary>
        /// <param name="taskNameCriteria">The task name criteria.</param>
        /// <returns></returns>
        List<string> GetTaskNamesByCommitDescription(Regex[] taskNameCriteria);
    }
}