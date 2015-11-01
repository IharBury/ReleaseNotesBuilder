namespace ReleaseNotesBuilder.GitHub
{
    public interface IGitHubConfiguration
    {
        string OwnerName { get; set; }
        string AccessToken { get; set; }
    }
}