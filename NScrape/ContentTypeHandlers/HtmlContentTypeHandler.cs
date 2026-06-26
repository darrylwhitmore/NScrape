using System.Net;
using NScrape.Interfaces;
using NScrape.Responses;

namespace NScrape.ContentTypeHandlers;

/// <summary>
/// Handles HTTP responses with content types related to HTML.
/// </summary>
/// <remarks>
/// This class is responsible for processing HTTP responses with content types that match
/// the specified prefix for HTML content. It creates instances of <see cref="NScrape.Responses.HtmlWebResponse" />
/// to represent the response data.
/// </remarks>
public class HtmlContentTypeHandler : ContentTypeHandlerBase {

	/// <summary>
	/// Initializes a new instance of the <see cref="NScrape.ContentTypeHandlers.HtmlContentTypeHandler"/> class.
	/// </summary>
	/// <param name="contentTypePrefix">
	/// The prefix of the content type that this handler will process, typically related to HTML content.
	/// </param>
	/// <remarks>
	/// This constructor sets the content type prefix for the handler, enabling it to identify
	/// and handle HTTP responses with content types matching the specified prefix.
	/// </remarks>
	public HtmlContentTypeHandler( string contentTypePrefix ) : base( contentTypePrefix ) {
	}

	/// <summary>
	/// Creates an instance of <see cref="NScrape.Responses.HtmlWebResponse" /> based on the provided
	/// <see cref="System.Net.HttpWebResponse" />.
	/// </summary>
	/// <param name="httpWebResponse">The HTTP web response to process.</param>
	/// <returns>
	/// An instance of <see cref="NScrape.Interfaces.IWebResponse" /> representing the HTML response.
	/// </returns>
	/// <remarks>
	/// This method overrides <see cref="NScrape.ContentTypeHandlers.ContentTypeHandlerBase.CreateResponseCore" />.
	/// It processes the given <see cref="System.Net.HttpWebResponse" /> and wraps it in an
	/// <see cref="NScrape.Responses.HtmlWebResponse" /> object, indicating whether the response was successful.
	/// </remarks>
	protected override IWebResponse CreateResponseCore( HttpWebResponse httpWebResponse ) => new HtmlWebResponse( true, httpWebResponse );
}
