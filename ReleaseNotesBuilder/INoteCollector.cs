using System.Collections.Generic;

namespace ReleaseNotesBuilder
{
    public interface INoteCollector
    {
        string RepositoryName { get; set; }
        string BranchName { get; set; }
        string TagName { get; set; }
        string[] TaskPrefixes { get; set; }
        List<Note> Collect();
    }
}