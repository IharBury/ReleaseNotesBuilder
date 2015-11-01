using System;
using ReleaseNotesBuilder.Arguments;
using ReleaseNotesBuilder.Formatting;
using ReleaseNotesBuilder.GitHub;
using ReleaseNotesBuilder.Jira;
using ReleaseNotesBuilder.TaskReferences;

namespace ReleaseNotesBuilder
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            var programConfiguration = new ProgramConfiguration(
                new JiraConfiguration(),
                new GitHubConfiguration(),
                new TaskReferenceByPrefixExtractorConfiguration(),
                new RazorTemplateNoteFormatterConfiguration());
            var argumentParser = new ArgumentParser(programConfiguration);

            try
            {
                argumentParser.Parse(args);
            }
            catch (ArgumentParsingException exception)
            {
                Console.Error.WriteLine(exception.Message);
                Console.WriteLine("Try `--help' for more information.");
                return 1;
            }
            catch (HelpRequestedException)
            {
                argumentParser.WriteHelp(Console.Out);
                return 2;
            }

            var gitHub = new GitHubClient();
            var jira = new JiraClient();
            var razorTemplateNoteFormatter = new RazorTemplateNoteFormatter(Console.Out);
            var noteCollector = new NoteCollector(gitHub, jira, razorTemplateNoteFormatter);
            noteCollector.Collect();
            return 0;
        }
    }
}