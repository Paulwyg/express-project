using System;
using System.Runtime.Serialization;

using JetBrains.Annotations;

namespace Elysium
{
    [Serializable]
    public class InvalidStyleException : Exception
    {
        [PublicAPI]
        public InvalidStyleException()
        {
        }

        [PublicAPI]
        public InvalidStyleException(string message)
            : base(message)
        {
        }

        [PublicAPI]
        public InvalidStyleException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        [PublicAPI]
        protected InvalidStyleException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}