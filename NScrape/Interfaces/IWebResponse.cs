using System;
using NScrape.Responses;

namespace NScrape.Interfaces;

/// <summary>
/// Defines the contract for a web response.
/// </summary>
public interface IWebResponse {
	/// <summary>
	/// Gets the type of the response.
	/// </summary>
	WebResponseType ResponseType { get; }

	/// <summary>
	/// Gets the URL of the response.
	/// </summary>
	Uri ResponseUrl { get; }

	/// <summary>
	/// Gets a value indicating whether the response is considered successful.
	/// </summary>
	bool Success { get; }
}