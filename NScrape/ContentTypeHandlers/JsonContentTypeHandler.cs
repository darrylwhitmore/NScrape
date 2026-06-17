using System.Net;
using NScrape.Interfaces;
using NScrape.Responses;

namespace NScrape.ContentTypeHandlers;

/// <summary>
/// Handles HTTP responses with JSON content types.
/// </summary>
/// <remarks>
/// This class is responsible for processing HTTP responses that have a content type matching
/// the specified JSON content type prefix. It creates instances of <see cref="NScrape.Responses.JsonWebResponse"/>
/// to represent the response data.
/// </remarks>
public class JsonContentTypeHandler : ContentTypeHandlerBase {

	/// <summary>
	/// Initializes a new instance of the <see cref="NScrape.ContentTypeHandlers.JsonContentTypeHandler"/> class.
	/// </summary>
	/// <param name="contentTypePrefix">
	/// The prefix of the JSON content type that this handler will process.
	/// </param>
	/// <remarks>
	/// This constructor sets the content type prefix for the handler, enabling it to identify
	/// and handle HTTP responses with JSON content types.
	/// </remarks>
	public JsonContentTypeHandler( string contentTypePrefix ) : base( contentTypePrefix ) {
	}
	
	/// <summary>
	/// Creates a new instance of <see cref="NScrape.Responses.JsonWebResponse"/> based on the provided HTTP response.
	/// </summary>
	/// <param name="httpWebResponse">The HTTP response to process.</param>
	/// <returns>
	/// An instance of <see cref="NScrape.Responses.JsonWebResponse"/> representing the processed response.
	/// </returns>
	/// <remarks>
	/// This method is overridden to handle HTTP responses with JSON-related content types.
	/// It ensures that the response is encapsulated in a <see cref="NScrape.Responses.JsonWebResponse"/> object.
	/// </remarks>
	public override IWebResponse CreateResponse( HttpWebResponse httpWebResponse ) => new JsonWebResponse( true, httpWebResponse );
}