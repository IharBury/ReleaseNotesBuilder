using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace ReleaseNotesBuilder.Formatting
{
    public class RazorTemplateNoteFormatter : INoteFormatter, IRazorTemplateNoteFormatterConfigurer
    {
        private readonly TextWriter writer;

        public RazorTemplateNoteFormatter(TextWriter writer)
        {
            this.writer = writer;
        }

        public string TemplateName { get; set; }

        public void Format(IEnumerable<Note> notes)
        {
            var templatePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, 
                "Formatting", 
                "Templates",
                string.Format("{0}.cshtml", TemplateName));
            var templateBody = File.ReadAllText(templatePath);

            var config = new TemplateServiceConfiguration
            {
                DisableTempFileLocking = true,
                CachingProvider = new DefaultCachingProvider(t => { })
            };

            // loads the files in-memory (gives the templates full-trust permissions)
            //disables the warnings
            // Use the config
            Razor.SetTemplateService(new TemplateService(config));

            var formattedNotes = Razor.Parse(templateBody, new ReportModel
            {
                Notes = notes.ToList()
            });

            writer.WriteLine(formattedNotes);
        }
    }
}