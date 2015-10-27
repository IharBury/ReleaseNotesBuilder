using System;
using System.Runtime.Serialization;

namespace ReleaseNotesBuilder.Arguments
{
    [Serializable]
    public class HelpRequestedException : Exception
    {
        public HelpRequestedException()
        {
        }

        public HelpRequestedException(string message) : base(message)
        {
        }

        public HelpRequestedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected HelpRequestedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
