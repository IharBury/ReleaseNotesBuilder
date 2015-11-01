namespace ReleaseNotesBuilder.Jira
{
    public interface IJiraConfiguration
    {
        string UserName { get; set; }
        string Password { get; set; }
    }
}