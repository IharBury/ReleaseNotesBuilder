using System;
using System.IO;

using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;

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

        public string TemplateName { get; set; }

        public string Build()
        {
            var notes = notesBuilder.Build();
            var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates",
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

            return Razor.Parse(templateBody, new ReportModel
            {
                Notes = notes
            });
        }
    }
}