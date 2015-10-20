namespace ReleaseNotesBuilder.Providers.GitHub.Client
{
    public class LinkedResponsePayload<T>
    {
        public T Data { get; set; }
        public bool NextPageAvailable { get; set; }
    }
}