namespace ReleaseNotesBuilder.Builders
{
    public class TemplateBuilderConfiguration
    {
        public string RepositoryName { get; set; }
        public string BranchName { get; set; }
        public string TagName { get; set; }
        public string[] TaskPrefixes { get; set; }
        public string TemplateName { get; set; }

    }
}