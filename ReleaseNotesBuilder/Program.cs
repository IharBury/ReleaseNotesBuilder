﻿using System;
using ReleaseNotesBuilder.Arguments;
using ReleaseNotesBuilder.Formatting;
using ReleaseNotesBuilder.GitHub;
using ReleaseNotesBuilder.Jira;
using ReleaseNotesBuilder.TaskReferences;

namespace ReleaseNotesBuilder
{
    public class Program : IProgramConfigurer
    {
        private readonly IGitHubClient gitHub;

        public Program(
            IGitHubClient gitHub, 
            ITaskReferenceByPrefixExtractorConfigurer taskReferenceExtractor, 
            IJiraConfigurer jira, 
            IRazorTemplateNoteFormatterConfigurer noteFormatter)
        {
            if (jira == null)
                throw new ArgumentNullException("jira");
            if (gitHub == null)
                throw new ArgumentNullException("gitHub");
            if (taskReferenceExtractor == null)
                throw new ArgumentNullException("taskReferenceExtractor");
            if (noteFormatter == null)
                throw new ArgumentNullException("noteFormatter");

            this.gitHub = gitHub;
            TaskReferenceExtractor = taskReferenceExtractor;
            Jira = jira;
            NoteFormatter = noteFormatter;
        }

        public IJiraConfigurer Jira { get; private set; }

        public IGitHubConfigurer GitHub
        {
            get { return gitHub; }
        }

        public ITaskReferenceByPrefixExtractorConfigurer TaskReferenceExtractor { get; private set; }
        public IRazorTemplateNoteFormatterConfigurer NoteFormatter { get; private set; }

        public void Run()
        {
            gitHub.CollectNotes();
        }

        public static int Main(string[] args)
        {
            var taskReferenceExtractor = new TaskReferenceByPrefixExtractor();
            var jira = new JiraClient();
            var noteFormatter = new RazorTemplateNoteFormatter(Console.Out);
            var gitHub = new GitHubClient(taskReferenceExtractor, jira, noteFormatter);
            var program = new Program(gitHub, taskReferenceExtractor, jira, noteFormatter);

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