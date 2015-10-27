using System.Collections.Generic;
using System.Linq;
using NDesk.Options;

namespace ReleaseNotesBuilder.Arguments
{
    public class ArgumentParser
    {
        private readonly IProgramConfiguration configuration;

        public ArgumentParser(IProgramConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Parse(IEnumerable<string> arguments)
        {
            var requiredArguments = new[]
            {
                new RequiredUniqueParameter("gn=", "GitHub User Name", o => configuration.GitHub.OwnerName = o),
                new RequiredUniqueParameter("gt=", "GitHub Access Token", o => configuration.GitHub.AccessToken = o), 
                new RequiredUniqueParameter("jn=", "Jira User Name", o => configuration.Jira.UserName = o), 
                new RequiredUniqueParameter("jp=", "Jira Password", o => configuration.Jira.Password = o), 
                new RequiredUniqueParameter("rn=", "Repository Name", o => configuration.NoteCollector.RepositoryName = o), 
                new RequiredUniqueParameter("bn=", "Branch Name", o => configuration.NoteCollector.BranchName = o), 
                new RequiredUniqueParameter("tn=", "Tag Name", o => configuration.NoteCollector.TagName = o),
                new RequiredParameter(
                    "tp=", 
                    "Task Prefixes", 
                    o =>
                    {
                        foreach (var taskPrefix in ParseTaskPrefixes(o))
                            configuration.NoteCollector.TaskPrefixes.Add(taskPrefix);
                    }), 
                new RequiredUniqueParameter("tpn=", "Template Name", o => configuration.NoteFormatter.TemplateName = o)
            };

            var optionSet = new OptionSet();
            foreach (var requiredArgument in requiredArguments)
                requiredArgument.AddToOptionSet(optionSet);

            try
            {
                var unparsedArguments = optionSet.Parse(arguments);
                if (unparsedArguments.Any())
                    throw new ArgumentParsingException(string.Format("Unknown argument \"{0}\".", unparsedArguments.First()));
            }
            catch (OptionException exception)
            {
                throw new ArgumentParsingException(exception.Message, exception);
            }

            foreach (var requiredArgument in requiredArguments)
                requiredArgument.AssertSupplied();
        }

        private static string[] ParseTaskPrefixes(string arg)
        {
            return arg.Split(',');
        }
    }
}
