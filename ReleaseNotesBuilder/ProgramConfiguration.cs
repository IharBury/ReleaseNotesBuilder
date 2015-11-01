using System.Collections.Generic;
using ReleaseNotesBuilder.Formatting;
using ReleaseNotesBuilder.GitHub;
using ReleaseNotesBuilder.Jira;

namespace ReleaseNotesBuilder
{
    public class ProgramConfiguration : IProgramConfiguration
    {
        public IJiraConfiguration Jira
        {
            get { throw new System.NotImplementedException(); }
        }

        public IGitHubConfiguration GitHub
        {
            get { throw new System.NotImplementedException(); }
        }

        public ICollection<string> TaskPrefixes
        {
            get { throw new System.NotImplementedException(); }
        }

        public string TemplateName
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }
    }
}
