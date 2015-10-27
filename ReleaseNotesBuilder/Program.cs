using System;

using NDesk.Options;

using ReleaseNotesBuilder.Builders;
using ReleaseNotesBuilder.Builders.Notes;
using ReleaseNotesBuilder.Providers.GitHub;
using ReleaseNotesBuilder.Providers.Jira;

namespace ReleaseNotesBuilder
{
    class Program
    {

        static void Main(string[] args)
        {
            var jira = new Jira();
            var gitHub = new GitHub();
            var notesBuilder = new NotesBuilder(gitHub, jira);
            var templateBuilder = new TemplateBuilder(notesBuilder);

            var options = new OptionSet
            {
                { "gn:","GitHub User Name", (o) => gitHub.OwnerName = o },
                { "gt:","GitHub Access Token", o => gitHub.AccessToken = o },
                { "jn:","Jira User Name", o => jira.UserName = o },
                { "jp:","Jira Password", o => jira.Password = o },
                { "rn:","Repository Name", o => notesBuilder.RepositoryName = o },
                { "bn:","Branch Name", o => notesBuilder.BranchName = o },
                { "tn:","Tag Name", o => notesBuilder.TagName = o },
                { "tp:","Task Prefixes", o => notesBuilder.TaskPrefixes = ParseTaskPrefixes(o) },
                { "tpn:","Template Name", o => templateBuilder.TemplateName = o }
            };

            try
            {
                options.Parse(args);

                string documentBody = templateBuilder.Build();

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