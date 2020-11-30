using System;
using System.Runtime.Serialization;

namespace R6Sharp.Exceptions
{
    [Serializable]
    public class UnrecognizedDataException : Exception
    {
        public UnrecognizedDataException()
        {
        }

        public UnrecognizedDataException(string message) : base(message)
        {
        }

        public UnrecognizedDataException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnrecognizedDataException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}