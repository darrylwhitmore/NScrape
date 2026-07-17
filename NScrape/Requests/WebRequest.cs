using System;
using System.Collections.Specialized;
using System.Net;
using NScrape.Interfaces;

namespace NScrape.Requests;

/// <summary>
/// Provides the base implementation for classes which represent web requests.
/// </summary>
public abstract class WebRequest {
	/// <summary>
	/// Initializes a new instance of the <see cref="WebRequest"/> class.
	/// </summary>
	/// <param name="type">Specifies the type of web request.</param>
	protected WebRequest( WebRequestType type )
		: this( type, new Uri( "about:blank" ) ) {
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="WebRequest"/> class.
	/// </summary>
	/// <param name="type">Specifies the type of web request.</param>
	/// <param name="destination">Specifies the destination of the request.</param>
	protected WebRequest( WebRequestType type, Uri destination ) {
		Headers = new NameValueCollection();
		AutoRedirect = true;
		Type = type;
		Destination = destination;
	}

	/// <summary>
	/// Gets or sets a value specifying whether the request shall be automatically redirected.
	/// </summary>
	/// <remarks>
	/// If <b>true</b>, the request shall be automatically redirected if specified by the server; if <b>false</b>, the
	/// request shall return a <see cref="IRedirectedWebResponse"/>.
	/// </remarks>
	public bool AutoRedirect { get; set; }

	/// <summary>
	/// Gets or sets the destination of the request.
	/// </summary>
	public Uri Destination { get; set; }

	/// <summary>
	/// Gets the collection of headers that shall be sent with the request.
	/// </summary>
	/// <remarks>
	/// Headers may be sent with the request by adding them to the headers collection.
	/// </remarks>
	public NameValueCollection Headers { get; }

	/// <summary>
	/// Gets the type of the request.
	/// </summary>
	public WebRequestType Type { get; private set; }

	/// <summary>
	/// Gets or sets the proxy information used for the web request.
	/// </summary>
	/// <value>
	/// An <see cref="IWebProxy"/> instance that specifies the proxy settings for the request.
	/// </value>
	/// <remarks>
	/// Use this property to configure the proxy server through which the web request is sent.
	/// If no proxy is specified, the request will use the default system proxy settings.
	/// </remarks>
	public IWebProxy Proxy { get; set; }
}
