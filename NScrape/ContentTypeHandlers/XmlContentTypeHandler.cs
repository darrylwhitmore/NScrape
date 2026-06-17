using System.Net;
using NScrape.Interfaces;
using NScrape.Responses;

namespace NScrape.ContentTypeHandlers;

/// <summary>
/// Handles HTTP responses with XML content types.
/// </summary>
/// <remarks>
/// This class is responsible for processing HTTP responses where the content type matches
/// a specified XML content type prefix. It extends <see cref="ContentTypeHandlerBase"/> and
/// creates instances of <see cref="XmlWebResponse"/> for handling XML-specific responses.
/// </remarks>
public class XmlContentTypeHandler : ContentTypeHandlerBase {
	
	/// <summary>
	/// Initializes a new instance of the <see cref="NScrape.ContentTypeHandlers.XmlContentTypeHandler"/> class.
	/// </summary>
	/// <param name="contentTypePrefix">
	/// The prefix of the XML content type that this handler will process.
	/// </param>
	/// <remarks>
	/// This constructor configures the handler to process HTTP responses with content types
	/// that match the specified XML content type prefix. It ensures that the handler is
	/// capable of identifying and handling XML-specific responses.
	/// </remarks>
	public XmlContentTypeHandler( string contentTypePrefix ) : base( contentTypePrefix ) {
	}
	
	/// <summary>
	/// Creates an instance of <see cref="NScrape.Responses.XmlWebResponse"/> to handle XML content.
	/// </summary>
	/// <param name="httpWebResponse">The HTTP web response to be processed as XML content.</param>
	/// <returns>
	/// An <see cref="NScrape.Interfaces.IWebResponse"/> implementation that represents the XML response.
	/// </returns>
	/// <remarks>
	/// This method provides a specialized response type for XML content by wrapping the provided
	/// <see cref="System.Net.HttpWebResponse"/> in an <see cref="NScrape.Responses.XmlWebResponse"/> instance.
	/// </remarks>
	public override IWebResponse CreateResponse( HttpWebResponse httpWebResponse ) => new XmlWebResponse( true, httpWebResponse );
}