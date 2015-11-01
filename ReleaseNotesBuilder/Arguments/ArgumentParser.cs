using System.Collections.Generic;
using System.IO;
using System.Linq;
using NDesk.Options;

namespace ReleaseNotesBuilder.Arguments
{
    public class ArgumentParser
    {
        private readonly RequiredParameter[] requiredArguments;
        private readonly OptionSet optionSet;

        public ArgumentParser(IProgramConfiguration configuration)
        {
            requiredArguments = new[]
            {
                new RequiredUniqueParameter(
                    "gn=", 
                    "GitHub user name", 
                    value => configuration.Release.GitHub.OwnerName = value),
                new RequiredUniqueParameter(
                    "gt=", 
                    "GitHub access token", 
                    value => configuration.Release.GitHub.AccessToken = value),
                new RequiredUniqueParameter(
                    "jn=", 
                    "Jira user name", 
                    value => configuration.Release.Jira.UserName = value),
                new RequiredUniqueParameter(
                    "jp=", 
                    "Jira password", 
                    value => configuration.Release.Jira.Password = value),
                new RequiredUniqueParameter(
                    "rn=", 
                    "Repository name", 
                    value => configuration.Release.RepositoryName = value),
                new RequiredUniqueParameter(
                    "bn=", 
                    "Branch name", 
                    value => configuration.Release.BranchName = value),
                new RequiredUniqueParameter(
                    "tn=", 
                    "Tag name", 
                    value => configuration.Release.TagName = value),
                new RequiredParameter(
                    "tp=",
                    "Comma-separated task prefixes (can be specified multiple times)",
                    value =>
                    {
                        foreach (var taskPrefix in ParseTaskPrefixes(value))
                            configuration.Release.TaskPrefixes.Add(taskPrefix);
                    }),
                new RequiredUniqueParameter(
                    "tpn=",
                    "Template name", 
                    value => configuration.RazorTemplate.TemplateName = value)
            };

            optionSet = new OptionSet
            {
                {
                    "help",
                    "Displays list of parameters",
                    value =>
                    {
                        throw new HelpRequestedException();
                    }
                }
            };
            foreach (var requiredArgument in requiredArguments)
                requiredArgument.AddToOptionSet(optionSet);
        }

        public void Parse(IEnumerable<string> arguments)
        {
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

        public void WriteHelp(TextWriter writer)
        {
            optionSet.WriteOptionDescriptions(writer);
        }

        private static string[] ParseTaskPrefixes(string arg)
        {
            return arg.Split(',');
        }
    }
}
