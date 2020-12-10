using System;
using System.Runtime.Serialization;

namespace R6Sharp.Exceptions
{
    [Serializable]
    internal class ApiBadResponseException : Exception
    {
        public ApiBadResponseException()
        {
        }

        public ApiBadResponseException(string message) : base(message)
        {
        }

        public ApiBadResponseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ApiBadResponseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}