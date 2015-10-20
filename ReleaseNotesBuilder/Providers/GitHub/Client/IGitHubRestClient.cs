namespace ReleaseNotesBuilder.Providers.GitHub.Client
{
    public interface IGitHubRestClient
    {
        string OwnerName { get; }

        LinkedResponsePayload<T> GetResponse<T>(string link) where T : class, new();
    }
}
