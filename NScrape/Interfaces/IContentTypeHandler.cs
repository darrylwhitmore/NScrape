using System.Net;

namespace NScrape.Interfaces;

/// <summary>
/// Defines the contract for handling specific content types in HTTP responses.
/// </summary>
public interface IContentTypeHandler {
	/// <summary>
	/// The content type prefix this handler responds to, e.g. "text/html" or "image/".
	/// </summary>
	string ContentTypePrefix { get; }

	/// <summary>
	/// Creates the appropriate web response for the given HTTP response.
	/// </summary>
	IWebResponse CreateResponse( HttpWebResponse httpWebResponse );
}