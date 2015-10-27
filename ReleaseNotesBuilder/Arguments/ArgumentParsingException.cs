using System;
using System.Runtime.Serialization;

namespace ReleaseNotesBuilder.Arguments
{
    [Serializable]
    public class ArgumentParsingException : Exception
    {
        public ArgumentParsingException()
        {
        }

        public ArgumentParsingException(string message) : base(message)
        {
        }

        public ArgumentParsingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ArgumentParsingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
