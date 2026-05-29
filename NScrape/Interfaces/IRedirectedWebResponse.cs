using System;
using NScrape.Requests;

namespace NScrape.Interfaces;

/// <summary>
/// Defines an interface for a web response that includes details about a redirection.
/// </summary>
public interface IRedirectedWebResponse : IWebResponse {
	/// <summary>
	/// Gets the redirect URL
	/// </summary>
	Uri RedirectUrl { get; }

	/// <summary>
	/// Gets the original web request.
	/// </summary>
	WebRequest WebRequest { get; }
}