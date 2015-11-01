namespace ReleaseNotesBuilder.Jira
{
    public interface IJiraConfigurer
    {
        string UserName { get; set; }
        string Password { get; set; }
    }
}