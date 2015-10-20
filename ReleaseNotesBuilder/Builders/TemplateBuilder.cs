using System;
using System.IO;

using RazorEngine;

using ReleaseNotesBuilder.Builders.Notes;
using ReleaseNotesBuilder.Model;

namespace ReleaseNotesBuilder.Builders
{
    public class TemplateBuilder
    {
        private readonly INotesBuilder notesBuilder;

        public TemplateBuilder(INotesBuilder notesBuilder)
        {
            this.notesBuilder = notesBuilder;
        }

        public string Build(string repositoryName, string branchName, string tagName, string[] taskPrefixes, string templateName)
        {
            var notes = notesBuilder.Build(repositoryName, branchName, tagName, taskPrefixes);
            var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates",
                string.Format("{0}.cshtml", templateName));
            var templateBody = File.ReadAllText(templatePath);
            return Razor.Parse(templateBody, new ReportModel
            {
                Notes = notes
            });
        }
    }
}