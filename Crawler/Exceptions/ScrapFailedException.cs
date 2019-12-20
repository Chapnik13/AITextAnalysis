using System;
using System.Runtime.Serialization;

namespace Crawler.Exceptions
{
    [Serializable]
    public class ScrapFailedException : Exception
    {
        public static ScrapFailedException CreateInstance()
        {
            return new ScrapFailedException();
        }

        private ScrapFailedException()
        {
        }

        public ScrapFailedException(string? message) : base(message)
        {
        }

        public ScrapFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }
        protected ScrapFailedException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {

        }
    }
}