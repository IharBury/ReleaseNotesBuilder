using System.Collections.Generic;
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
                new RequiredUniqueArgument("gn=", "GitHub User Name", o => configuration.GitHub.OwnerName = o),
                new RequiredUniqueArgument("gt=", "GitHub Access Token", o => configuration.GitHub.AccessToken = o), 
                new RequiredUniqueArgument("jn=", "Jira User Name", o => configuration.Jira.UserName = o), 
                new RequiredUniqueArgument("jp=", "Jira Password", o => configuration.Jira.Password = o), 
                new RequiredUniqueArgument("rn=", "Repository Name", o => configuration.NoteCollector.RepositoryName = o), 
                new RequiredUniqueArgument("bn=", "Branch Name", o => configuration.NoteCollector.BranchName = o), 
                new RequiredUniqueArgument("tn=", "Tag Name", o => configuration.NoteCollector.TagName = o),
                new RequiredArgument(
                    "tp=", 
                    "Task Prefixes", 
                    o =>
                    {
                        foreach (var taskPrefix in ParseTaskPrefixes(o))
                            configuration.NoteCollector.TaskPrefixes.Add(taskPrefix);
                    }), 
                new RequiredUniqueArgument("tpn=", "Template Name", o => configuration.NoteFormatter.TemplateName = o)
            };

            var optionSet = new OptionSet();
            foreach (var requiredArgument in requiredArguments)
                requiredArgument.AddToOptionSet(optionSet);

            try
            {
                optionSet.Parse(arguments);
            }
            catch (OptionException exception)
            {
                throw new ParameterParsingException(exception.Message, exception);
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
