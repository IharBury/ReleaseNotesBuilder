using System;

namespace ReleaseNotesBuilder.Arguments
{
    public class RequiredUniqueParameter : RequiredParameter
    {
        public RequiredUniqueParameter(string prototype, string description, Action<string> action)
            : base(prototype, description, action)
        {
        }

        protected override void HandleDuplicateValue()
        {
            throw new ArgumentParsingException(string.Format("{0} parameter is supplied more than once.", Description));
        }
    }
}
