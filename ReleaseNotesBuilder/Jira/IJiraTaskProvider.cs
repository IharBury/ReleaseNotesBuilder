namespace ReleaseNotesBuilder.Jira
{
    public interface IJiraTaskProvider
    {
        TaskDataModel GetTask(string taskName);
    }
}