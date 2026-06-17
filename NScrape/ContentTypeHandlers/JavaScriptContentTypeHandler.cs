using System.Net;
using NScrape.Interfaces;
using NScrape.Responses;

namespace NScrape.ContentTypeHandlers;

/// <summary>
/// Handles HTTP responses with JavaScript content types.
/// </summary>
/// <remarks>
/// This class is responsible for processing HTTP responses where the content type matches
/// a JavaScript-related prefix (e.g., "application/javascript"). It creates instances of
/// <see cref="NScrape.Responses.JavaScriptWebResponse"/> to represent the response data.
/// </remarks>
public class JavaScriptContentTypeHandler : ContentTypeHandlerBase {

	/// <summary>
	/// Initializes a new instance of the <see cref="NScrape.ContentTypeHandlers.JavaScriptContentTypeHandler"/> class.
	/// </summary>
	/// <param name="contentTypePrefix">
	/// The prefix of the JavaScript-related content type that this handler will process.
	/// </param>
	/// <remarks>
	/// This constructor specifies the content type prefix for JavaScript-related HTTP responses,
	/// enabling the handler to identify and process such responses.
	/// </remarks>
	public JavaScriptContentTypeHandler( string contentTypePrefix ) : base( contentTypePrefix ) {
	}
	
	/// <summary>
	/// Creates a new instance of <see cref="NScrape.Responses.JavaScriptWebResponse"/> based on the provided HTTP response.
	/// </summary>
	/// <param name="httpWebResponse">The HTTP response to process.</param>
	/// <returns>
	/// An instance of <see cref="NScrape.Responses.JavaScriptWebResponse"/> representing the processed response.
	/// </returns>
	/// <remarks>
	/// This method is overridden to handle HTTP responses with JavaScript-related content types.
	/// It ensures that the response is encapsulated in a <see cref="NScrape.Responses.JavaScriptWebResponse"/> object.
	/// </remarks>
	public override IWebResponse CreateResponse( HttpWebResponse httpWebResponse ) => new JavaScriptWebResponse( true, httpWebResponse );
}