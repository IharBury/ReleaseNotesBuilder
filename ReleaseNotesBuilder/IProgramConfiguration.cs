using ReleaseNotesBuilder.GitHub;
using ReleaseNotesBuilder.Jira;

namespace ReleaseNotesBuilder
{
    public interface IProgramConfiguration
    {
        JiraClient Jira { get; }
        GitHubClient GitHub { get; }
        NoteCollector NoteCollector { get; }
        NoteFormatter NoteFormatter { get; }
    }
}