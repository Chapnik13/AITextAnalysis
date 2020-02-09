using System;
using System.Runtime.Serialization;

namespace Crawler.Exceptions
{
	[Serializable]
	public class DecileInvalidArgumentsAmountException : Exception
	{
		public static DecileInvalidArgumentsAmountException CreateInstance()
		{
			return new DecileInvalidArgumentsAmountException();
		}

		public DecileInvalidArgumentsAmountException()
		{
		}

		public DecileInvalidArgumentsAmountException(string? message) : base(message)
		{
		}

		public DecileInvalidArgumentsAmountException(string message, Exception innerException) : base(message, innerException)
		{
		}
		protected DecileInvalidArgumentsAmountException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{

		}
	}
}
