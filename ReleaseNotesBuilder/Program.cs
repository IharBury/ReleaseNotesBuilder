using System;
using ReleaseNotesBuilder.Arguments;
using ReleaseNotesBuilder.GitHub;
using ReleaseNotesBuilder.Jira;

namespace ReleaseNotesBuilder
{
    public class Program : IProgramConfiguration
    {
        public Program()
        {
            Jira = new JiraClient();
            GitHub = new GitHubClient();
            NoteCollector = new NoteCollector(GitHub, Jira);
            NoteFormatter = new NoteFormatter();
        }


        public JiraClient Jira { get; private set; }
        public GitHubClient GitHub { get; private set; }
        public INoteCollector NoteCollector { get; private set; }
        public NoteFormatter NoteFormatter { get; private set; }

        public void Run()
        {
            var notes = NoteCollector.Collect();
            var formattedNotes = NoteFormatter.Format(notes);
            Console.WriteLine(formattedNotes);
        }


        public static int Main(string[] args)
        {
            var program = new Program();
            var argumentParser = new ArgumentParser(program);

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

            program.Run();
            return 0;
        }
    }
}