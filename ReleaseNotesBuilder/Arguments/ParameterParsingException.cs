using System;
using System.Runtime.Serialization;

namespace ReleaseNotesBuilder.Arguments
{
    [Serializable]
    public class ParameterParsingException : Exception
    {
        public ParameterParsingException()
        {
        }

        public ParameterParsingException(string message) : base(message)
        {
        }

        public ParameterParsingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ParameterParsingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
