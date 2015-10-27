using System;
using NDesk.Options;

namespace ReleaseNotesBuilder.Arguments
{
    public class RequiredUniqueArgument : RequiredArgument
    {
        public RequiredUniqueArgument(string prototype, string description, Action<string> action)
            : base(prototype, description, action)
        {
        }

        protected override void HandleDuplicateValue()
        {
            throw new ParameterParsingException(string.Format("{0} parameter is supplied more than once.", Description));
        }
    }
}
