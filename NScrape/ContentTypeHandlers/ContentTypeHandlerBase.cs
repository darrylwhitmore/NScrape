using System;
using System.Net;
using NScrape.Interfaces;

namespace NScrape.ContentTypeHandlers;

/// <summary>
/// Serves as the base class for handling specific content types in HTTP responses.
/// </summary>
/// <remarks>
/// This abstract class provides a foundation for implementing content type handlers that process
/// HTTP responses based on their content type prefix. Derived classes must specify the content type
/// prefix and implement the logic for creating the appropriate web response.
/// </remarks>
public abstract class ContentTypeHandlerBase : IContentTypeHandler {
	/// <summary>
	/// Initializes a new instance of the <see cref="NScrape.ContentTypeHandlers.ContentTypeHandlerBase"/> class.
	/// </summary>
	/// <param name="contentTypePrefix">
	/// The prefix of the content type that this handler will process.
	/// </param>
	/// <remarks>
	/// This constructor sets the content type prefix for the handler, enabling it to identify
	/// and handle HTTP responses with matching content types.
	/// </remarks>
	protected ContentTypeHandlerBase( string contentTypePrefix ) {
		ContentTypePrefix = contentTypePrefix;
	}

	/// <summary>
	/// Gets the prefix of the content type that this handler is responsible for processing.
	/// </summary>
	/// <remarks>
	/// The content type prefix is used to determine whether this handler can process a specific
	/// HTTP response based on its content type. Derived classes must specify the appropriate
	/// prefix during initialization.
	/// </remarks>
	public string ContentTypePrefix { get; }

	/// <summary>
	/// Creates an <see cref="NScrape.Interfaces.IWebResponse"/> instance based on the provided 
	/// <see cref="System.Net.HttpWebResponse"/>.
	/// </summary>
	/// <param name="httpWebResponse">The HTTP web response to process.</param>
	/// <returns>An instance of <see cref="NScrape.Interfaces.IWebResponse"/> representing the processed response.</returns>
	/// <exception cref="System.ArgumentNullException">
	/// Thrown when <paramref name="httpWebResponse"/> is <c>null</c>.
	/// </exception>
	/// <remarks>
	/// This method delegates the actual response creation to the <see cref="CreateResponseCore"/> method, 
	/// which must be implemented by derived classes.
	/// </remarks>
	public IWebResponse CreateResponse( HttpWebResponse httpWebResponse ) {
		ArgumentNullException.ThrowIfNull( httpWebResponse );

		return CreateResponseCore( httpWebResponse );
	}

	/// <summary>
	/// When overridden in a derived class, creates an <see cref="IWebResponse"/> instance 
	/// based on the provided <see cref="HttpWebResponse"/>.
	/// </summary>
	/// <param name="httpWebResponse">The HTTP web response to be processed.</param>
	/// <returns>An <see cref="IWebResponse"/> instance representing the processed response.</returns>
	/// <remarks>
	/// This method must be implemented by derived classes to define specific handling logic 
	/// for the content type associated with the handler.
	/// </remarks>
	protected abstract IWebResponse CreateResponseCore( HttpWebResponse httpWebResponse );
}
