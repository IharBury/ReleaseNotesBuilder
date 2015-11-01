namespace ReleaseNotesBuilder.Jira
{
    public interface IJiraTaskNoteProvider
    {
        TaskDataModel GetTask(string taskName);
    }
}