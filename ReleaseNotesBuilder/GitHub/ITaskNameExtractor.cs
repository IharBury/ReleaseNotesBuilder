using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ReleaseNotesBuilder.GitHub
{
    public interface ITaskNameExtractor
    {
        /// <summary>
        ///     Gets the task names by commit description.
        /// </summary>
        /// <param name="repositoryName">Name of the repository.</param>
        /// <param name="branchName">Name of the branch.</param>
        /// <param name="tagName">Name of the tag.</param>
        /// <param name="taskNameCriteria">The task name criteria.</param>
        /// <returns></returns>
        List<string> GetTaskNamesByCommitDescription(
            string repositoryName, string 
                branchName, 
            string tagName,
            Regex[] taskNameCriteria);
    }
}