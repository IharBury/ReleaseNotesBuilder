using System;

using NDesk.Options;

using ReleaseNotesBuilder.Builders;
using ReleaseNotesBuilder.Builders.Notes;

namespace ReleaseNotesBuilder
{
    class Program
    {

        static void Main(string[] args)
        {
            var notesBuilderConfiguration = new NotesBuilderConfiguration();
            var templateBuilderConfiguration = new TemplateBuilderConfiguration();

            var options = new OptionSet
            {
                { "gn:","GitHub User Name", (o) => notesBuilderConfiguration.GithubOwnerName = o },
                { "gt:","GitHub Access Token", o => notesBuilderConfiguration.GithubAccessToken = o },
                { "jn:","Jira User Name", o => notesBuilderConfiguration.JiraUsername = o },
                { "jp:","Jira Password", o => notesBuilderConfiguration.JiraPassword = o },
                { "rn:","Repository Name", o => templateBuilderConfiguration.RepositoryName = o },
                { "bn:","Branch Name", o => templateBuilderConfiguration.BranchName = o },
                { "tn:","Tag Name", o => templateBuilderConfiguration.TagName = o },
                { "tp:","Task Prefixes", o => templateBuilderConfiguration.TaskPrefixes = ParseTaskPrefixes(o) },
                { "tpn:","Template Name", o => templateBuilderConfiguration.TemplateName = o }
            };

            try
            {
                options.Parse(args);

                var notesBuilder = new NotesBuilder(notesBuilderConfiguration);
                var templateBuilder = new TemplateBuilder(notesBuilder);
                string documentBody = templateBuilder.Build(templateBuilderConfiguration.RepositoryName,
                    templateBuilderConfiguration.BranchName,
                    templateBuilderConfiguration.TagName,
                    templateBuilderConfiguration.TaskPrefixes,
                    templateBuilderConfiguration.TemplateName);

                Console.WriteLine(documentBody);

                Console.ReadKey();
            }
            catch (OptionException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `--help' for more information.");
            }

        }

        private static string[] ParseTaskPrefixes(string arg)
        {
            return arg.Split(',');
        }
    }
}