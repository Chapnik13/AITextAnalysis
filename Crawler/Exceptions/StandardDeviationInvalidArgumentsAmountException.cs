using System;
using System.Runtime.Serialization;

namespace Crawler.Exceptions
{
	[Serializable]
	public class StandardDeviationInvalidArgumentsAmountException : Exception
	{
		public static StandardDeviationInvalidArgumentsAmountException CreateInstance()
		{
			return new StandardDeviationInvalidArgumentsAmountException();
		}

		public StandardDeviationInvalidArgumentsAmountException()
		{
		}

		public StandardDeviationInvalidArgumentsAmountException(string? message) : base(message)
		{
		}

		public StandardDeviationInvalidArgumentsAmountException(string message, Exception innerException) : base(message, innerException)
		{
		}
		protected StandardDeviationInvalidArgumentsAmountException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{

		}
	}
}
