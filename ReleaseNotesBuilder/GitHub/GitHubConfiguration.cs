namespace ReleaseNotesBuilder.GitHub
{
    public class GitHubConfiguration : IGitHubConfigurer
    {
        public string OwnerName { get; set; }
        public string AccessToken { get; set; }
        public string RepositoryName { get; set; }
        public string BranchName { get; set; }
        public string TagName { get; set; }
    }
}
