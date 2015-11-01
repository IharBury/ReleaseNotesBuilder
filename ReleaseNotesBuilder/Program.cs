using System;
using ReleaseNotesBuilder.Arguments;
using ReleaseNotesBuilder.Formatting;
using ReleaseNotesBuilder.GitHub;
using ReleaseNotesBuilder.Jira;
using ReleaseNotesBuilder.TaskReferences;

namespace ReleaseNotesBuilder
{
    public class Program : IProgramConfigurer
    {
        private readonly INoteCollector noteCollector;

        public Program(
            IJiraConfigurer jira,
            IGitHubConfigurer gitHub,
            ITaskReferenceByPrefixExtractorConfigurer taskReferenceExtractor,
            IRazorTemplateNoteFormatterConfigurer noteFormatter,
            INoteCollector noteCollector)
        {
            if (jira == null)
                throw new ArgumentNullException("jira");
            if (gitHub == null)
                throw new ArgumentNullException("gitHub");
            if (taskReferenceExtractor == null)
                throw new ArgumentNullException("taskReferenceExtractor");
            if (noteFormatter == null)
                throw new ArgumentNullException("noteFormatter");
            if (noteCollector == null)
                throw new ArgumentNullException("noteCollector");

            Jira = jira;
            GitHub = gitHub;
            TaskReferenceExtractor = taskReferenceExtractor;
            NoteFormatter = noteFormatter;
            this.noteCollector = noteCollector;
        }

        public IJiraConfigurer Jira { get; private set; }
        public IGitHubConfigurer GitHub { get; private set; }
        public ITaskReferenceByPrefixExtractorConfigurer TaskReferenceExtractor { get; private set; }
        public IRazorTemplateNoteFormatterConfigurer NoteFormatter { get; private set; }

        public void Run()
        {
            noteCollector.Collect();
        }

        public static int Main(string[] args)
        {
            var taskReferenceExtractor = new TaskReferenceByPrefixExtractor();
            var gitHub = new GitHubClient(taskReferenceExtractor);
            var jira = new JiraClient();
            var noteFormatter = new RazorTemplateNoteFormatter(Console.Out);
            var noteCollector = new NoteCollector(gitHub, jira, noteFormatter);
            var program = new Program(jira, gitHub, taskReferenceExtractor, noteFormatter, noteCollector);

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