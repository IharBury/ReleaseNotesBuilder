﻿using System;
using ReleaseNotesBuilder.Arguments;
using ReleaseNotesBuilder.Formatting;
using ReleaseNotesBuilder.GitHub;
using ReleaseNotesBuilder.Jira;

namespace ReleaseNotesBuilder
{
    public class Program : IProgramConfiguration
    {
        private readonly INoteCollector noteCollector;
        private readonly IRazorTemplateNoteFormatter razorTemplateNoteFormatter;

        public Program(INoteCollector noteCollector, IRazorTemplateNoteFormatter razorTemplateNoteFormatter)
        {
            this.noteCollector = noteCollector;
            this.razorTemplateNoteFormatter = razorTemplateNoteFormatter;
        }

        public IReleaseConfiguration Release
        {
            get { return noteCollector; }
        }

        public IRazorTemplateConfiguration RazorTemplate
        {
            get { return razorTemplateNoteFormatter; }
        }

        public void Run()
        {
            noteCollector.Collect();
        }


        public static int Main(string[] args)
        {
            var gitHub = new GitHubClient();
            var jira = new JiraClient();
            var razorTemplateNoteFormatter = new RazorTemplateNoteFormatter(Console.Out);
            var noteCollector = new NoteCollector(gitHub, jira, razorTemplateNoteFormatter);
            var program = new Program(noteCollector, razorTemplateNoteFormatter);
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