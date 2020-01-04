using System;
using System.Runtime.Serialization;

namespace Crawler.Exceptions
{
    [Serializable]
    public class SelectorNotFoundException : Exception
    {
        public SelectorNotFoundException()
        {
        }

        public SelectorNotFoundException(string? message) : base(message)
        {
        }

        public SelectorNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
        protected SelectorNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {

        }
    }
}
