using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using NScrape.ContentTypeHandlers;
using NScrape.Interfaces;
using NScrape.Responses;

namespace NScrape;

/// <summary>
/// Represents a factory for creating instances of <see cref="NScrape.Interfaces.IWebResponse"/> 
/// based on the provided <see cref="System.Net.HttpWebResponse"/>.
/// </summary>
/// <remarks>
/// This class allows customization of response creation by supporting a collection of 
/// <see cref="NScrape.Interfaces.IContentTypeHandler"/> implementations. These handlers 
/// can be used to process specific content types.
/// </remarks>
public class WebResponseFactory : IWebResponseFactory {
	private static readonly IEnumerable<IContentTypeHandler> defaultHandlers = [
		new BinaryContentTypeHandler( "application/gzip" ),
		new BinaryContentTypeHandler( "application/msword" ),
		new BinaryContentTypeHandler( "application/octet-stream" ),
		new BinaryContentTypeHandler( "application/pdf" ),
		new BinaryContentTypeHandler( "application/vnd.ms-excel" ),
		new BinaryContentTypeHandler( "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" ),
		new BinaryContentTypeHandler( "application/vnd.openxmlformats-officedocument.wordprocessingml.document" ),
		new BinaryContentTypeHandler( "application/x-dosexec" ),
		new BinaryContentTypeHandler( "application/x-msdos-program" ),
		new BinaryContentTypeHandler( "application/x-zip-compressed" ),
		new BinaryContentTypeHandler( "application/zip" ),
		new BinaryContentTypeHandler( "audio/" ),
		new BinaryContentTypeHandler( "font/" ),
		new BinaryContentTypeHandler( "image/" ),
		new BinaryContentTypeHandler( "video/" ),

		new HtmlContentTypeHandler( "application/xhtml+xml" ),
		new HtmlContentTypeHandler( "text/html" ),

		new JavaScriptContentTypeHandler( "application/javascript" ),
		new JavaScriptContentTypeHandler( "application/x-javascript" ),
		new JavaScriptContentTypeHandler( "text/javascript" ),

		new JsonContentTypeHandler( "application/json" ),
		new JsonContentTypeHandler( "application/ld+json" ),
			
		new PlainTextContentTypeHandler( "text/plain" ),

		new XmlContentTypeHandler( "application/atom+xml" ),
		new XmlContentTypeHandler( "application/rss+xml" ),
		new XmlContentTypeHandler( "application/xml" ),
		new XmlContentTypeHandler( "text/xml" )
	];

	private readonly Dictionary<string, IContentTypeHandler> handlers;

	/// <summary>
	/// Initializes a new instance of the <see cref="NScrape.WebResponseFactory"/> class with default settings.
	/// </summary>
	/// <remarks>
	/// This constructor sets up the factory without any custom content type handlers. 
	/// It uses the default behavior for creating instances of <see cref="NScrape.Interfaces.IWebResponse"/>.
	/// </remarks>
	public WebResponseFactory() : this( [] ) { 
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="WebResponseFactory"/> class with the specified custom content type handlers.
	/// </summary>
	/// <param name="customHandlers">
	/// A collection of custom content type handlers to be used. These handlers will override the default handlers if they share the same content type prefix.
	/// </param>
	public WebResponseFactory( IEnumerable<IContentTypeHandler> customHandlers ) {
		// Add our default handlers
		handlers = defaultHandlers.ToDictionary( h => h.ContentTypePrefix, h => h, StringComparer.OrdinalIgnoreCase );

		// Add the user's custom handlers, replacing ours if so specified
		foreach ( var handler in customHandlers ) {
			handlers[handler.ContentTypePrefix] = handler;
		}
	}

	/// <summary>
	/// Creates an instance of <see cref="IWebResponse"/> based on the provided <see cref="HttpWebResponse"/>.
	/// </summary>
	/// <param name="httpWebResponse">The HTTP web response to be wrapped in an <see cref="IWebResponse"/> instance.</param>
	/// <returns>
	/// An implementation of <see cref="IWebResponse"/> that corresponds to the specified <paramref name="httpWebResponse"/>.
	/// </returns>
	public IWebResponse CreateResponse( HttpWebResponse httpWebResponse ) {
		if ( !httpWebResponse.Headers.AllKeys.Contains( CommonHeaders.ContentType ) ) {
			// The response is missing a content type.
			return new UnsupportedWebResponse( httpWebResponse.ResponseUri, string.Empty );
		}

		var contentType = httpWebResponse.Headers[CommonHeaders.ContentType];

		if ( contentType is null ) {
			// The content type header is present but has no value.
			return new UnsupportedWebResponse( httpWebResponse.ResponseUri, string.Empty );
		}

		var key = handlers.Keys.SingleOrDefault( k => contentType.StartsWith( k, StringComparison.OrdinalIgnoreCase ) );

		return key != null
			? handlers[key].CreateResponse( httpWebResponse )
			: new UnsupportedWebResponse( httpWebResponse.ResponseUri, contentType );
	}
}