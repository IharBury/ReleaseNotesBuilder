namespace ReleaseNotesBuilder.Providers.GitHub.Models
{
    public class LinkedResponsePayload<T>
    {
        public T Data { get; set; }
        public bool NextPageAvailable { get; set; }
    }
}