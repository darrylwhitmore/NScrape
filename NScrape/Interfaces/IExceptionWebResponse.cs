using System.Net;

namespace NScrape.Interfaces;

/// <summary>
/// Defines a contract for a web response that encapsulates an exception.
/// </summary>
public interface IExceptionWebResponse : IWebResponse {
	/// <summary>
	/// Gets the exception.
	/// </summary>
	WebException Exception { get; }
}