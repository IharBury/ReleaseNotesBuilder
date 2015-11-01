namespace ReleaseNotesBuilder.GitHub
{
    public interface IGitHubConfiguration
    {
        string OwnerName { get; set; }
        string AccessToken { get; set; }
        string RepositoryName { get; set; }
        string BranchName { get; set; }
        string TagName { get; set; }
    }
}