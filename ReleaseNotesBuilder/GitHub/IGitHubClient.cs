namespace ReleaseNotesBuilder.GitHub
{
    public interface IGitHubClient : IGitHubConfiguration, IGitHubCommitProvider, ITaskNameExtractor
    {
    }
}