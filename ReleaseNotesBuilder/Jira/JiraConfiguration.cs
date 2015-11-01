namespace ReleaseNotesBuilder.Jira
{
    public class JiraConfiguration : IJiraConfigurer
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
