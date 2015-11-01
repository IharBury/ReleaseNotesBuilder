using ReleaseNotesBuilder.Formatting;

namespace ReleaseNotesBuilder
{
    public interface IProgramConfiguration
    {
        IReleaseConfiguration Release { get; }
        IRazorTemplateConfiguration RazorTemplate { get; }
    }
}