namespace ReleaseNotesBuilder.GitHub
{
    public interface IGitHubClient : IGitHubConfigurer, IGitHubCommitProvider, ITaskNameExtractor
    {
    }
}