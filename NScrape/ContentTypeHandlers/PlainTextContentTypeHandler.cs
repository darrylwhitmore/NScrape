using System.Net;
using NScrape.Interfaces;
using NScrape.Responses;

namespace NScrape.ContentTypeHandlers;

/// <summary>
/// Handles HTTP responses with a plain text content type.
/// </summary>
/// <remarks>
/// This class is responsible for processing HTTP responses where the content type matches
/// the specified plain text prefix. It creates instances of <see cref="NScrape.Responses.PlainTextWebResponse" />
/// to represent the response data.
/// </remarks>
public class PlainTextContentTypeHandler : ContentTypeHandlerBase {
	
	/// <summary>
	/// Initializes a new instance of the <see cref="NScrape.ContentTypeHandlers.PlainTextContentTypeHandler"/> class.
	/// </summary>
	/// <param name="contentTypePrefix">
	/// The prefix of the plain text content type that this handler will process.
	/// </param>
	/// <remarks>
	/// This constructor sets the content type prefix for the handler, enabling it to identify
	/// and handle HTTP responses with plain text content types.
	/// </remarks>
	public PlainTextContentTypeHandler( string contentTypePrefix ) : base( contentTypePrefix ) {
	}

	/// <summary>
	/// Creates a response object for handling plain text content.
	/// </summary>
	/// <param name="httpWebResponse">The HTTP web response to be wrapped in a plain text response object.</param>
	/// <returns>
	/// An instance of <see cref="NScrape.Responses.PlainTextWebResponse"/> that represents the plain text response.
	/// </returns>
	/// <remarks>
	/// This method overrides the base implementation in <see cref="NScrape.ContentTypeHandlers.ContentTypeHandlerBase"/>
	/// to provide a specific response type for plain text content.
	/// </remarks>
	protected override IWebResponse CreateResponseCore( HttpWebResponse httpWebResponse ) => new PlainTextWebResponse( true, httpWebResponse );
}
