using System;
using NDesk.Options;

namespace ReleaseNotesBuilder.Arguments
{
    public class RequiredUniqueArgument
    {
        private readonly string prototype;
        private readonly string description;
        private readonly Action<string> action;
        private bool isSupplied;

        public RequiredUniqueArgument(string prototype, string description, Action<string> action)
        {
            this.prototype = prototype;
            this.description = description;
            this.action = action;
        }

        public void AddToOptionSet(OptionSet optionSet)
        {
            optionSet.Add(
                prototype,
                description,
                value =>
                {
                    if (isSupplied)
                        throw new ParameterParsingException(
                            string.Format("{0} parameter is supplied more than once.", description));

                    isSupplied = true;
                    action(value);
                });
        }

        public void AssertSupplied()
        {
            if (!isSupplied)
                throw new ParameterParsingException(string.Format("{0} parameter is missing.", description));
        }
    }
}
