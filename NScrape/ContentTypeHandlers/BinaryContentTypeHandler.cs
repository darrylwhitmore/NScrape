using System.Net;
using NScrape.Interfaces;
using NScrape.Responses;

namespace NScrape.ContentTypeHandlers;

/// <summary>
/// Handles HTTP responses with binary content types.
/// </summary>
/// <remarks>
/// This class is responsible for creating instances of <see cref="NScrape.Responses.BinaryWebResponse"/> 
/// for HTTP responses that match the specified binary content type prefix.
/// </remarks>
public class BinaryContentTypeHandler : ContentTypeHandlerBase {
	
	/// <summary>
	/// Initializes a new instance of the <see cref="NScrape.ContentTypeHandlers.BinaryContentTypeHandler"/> class.
	/// </summary>
	/// <param name="contentTypePrefix">
	/// The prefix of the binary content type that this handler will process.
	/// </param>
	/// <remarks>
	/// This constructor sets the content type prefix for the handler, enabling it to identify
	/// and handle HTTP responses with matching binary content types.
	/// </remarks>
	public BinaryContentTypeHandler( string contentTypePrefix ): base( contentTypePrefix ) {
	}

	/// <summary>
	/// Creates a new instance of <see cref="NScrape.Responses.BinaryWebResponse"/> 
	/// based on the provided <see cref="System.Net.HttpWebResponse"/>.
	/// </summary>
	/// <param name="httpWebResponse">The HTTP web response to process.</param>
	/// <returns>
	/// An instance of <see cref="NScrape.Responses.BinaryWebResponse"/> representing the processed response.
	/// </returns>
	/// <remarks>
	/// This method overrides the base implementation in <see cref="NScrape.ContentTypeHandlers.ContentTypeHandlerBase"/> 
	/// to specifically handle binary content types.
	/// </remarks>
	protected override IWebResponse CreateResponseCore( HttpWebResponse httpWebResponse ) => new BinaryWebResponse( true, httpWebResponse );
}
