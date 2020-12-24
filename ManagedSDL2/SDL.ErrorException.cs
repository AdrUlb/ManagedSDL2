using System;
using System.Runtime.Serialization;

namespace ManagedSDL2
{
	public static partial class SDL
	{
		public class ErrorException : Exception
		{
			public ErrorException() { }
			public ErrorException(string? message) : base(message) { }
			public ErrorException(string? message, Exception? innerException) : base(message, innerException) { }
			public ErrorException(SerializationInfo info, StreamingContext context) : base(info, context) { }
		}
	}
}
