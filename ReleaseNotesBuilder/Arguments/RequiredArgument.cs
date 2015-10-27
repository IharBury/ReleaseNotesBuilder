using System;
using NDesk.Options;

namespace ReleaseNotesBuilder.Arguments
{
    public class RequiredArgument
    {
        private readonly string prototype;
        private readonly Action<string> action;
        private bool isSupplied;

        public RequiredArgument(string prototype, string description, Action<string> action)
        {
            this.prototype = prototype;
            Description = description;
            this.action = action;
        }

        protected string Description { get; private set; }

        public void AddToOptionSet(OptionSet optionSet)
        {
            optionSet.Add(
                prototype,
                Description,
                value =>
                {
                    if (isSupplied)
                        HandleDuplicateValue();

                    isSupplied = true;
                    action(value);
                });
        }

        public void AssertSupplied()
        {
            if (!isSupplied)
                throw new ParameterParsingException(string.Format("{0} parameter is missing.", Description));
        }

        protected virtual void HandleDuplicateValue()
        {
            // Do nothing.
        }
    }
}
