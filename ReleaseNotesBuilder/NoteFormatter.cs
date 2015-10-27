using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using ReleaseNotesBuilder.Templates;

namespace ReleaseNotesBuilder
{
    public class NoteFormatter
    {
        public string TemplateName { get; set; }

        public string Format(IEnumerable<Note> notes)
        {
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
                Notes = notes.ToList()
            });
        }
    }
}