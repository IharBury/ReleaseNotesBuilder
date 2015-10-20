namespace ReleaseNotesBuilder.Providers.Jira.Client
{
    public interface IJiraRestClient
    {
        T GetResponse<T>(string link) where T : class, new();
    }
}