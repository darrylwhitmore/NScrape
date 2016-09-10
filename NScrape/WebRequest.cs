using System;
using System.Collections.Specialized;

namespace NScrape {

    /// <summary>
	/// Provides the base implementation for classes which represent web requests.
    /// </summary>
    public abstract class WebRequest {
	    private const string XmlHttpRequestValue = "XMLHttpRequest";

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
		/// request shall return a <see cref="RedirectedWebResponse"/>.
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
		/// Gets or sets a value indicating whether the request shall attempt to mimic a JQuery request.
		/// </summary>
		/// <remarks>
		/// If <b>true</b>, the <b>X-Requested-With=XMLHttpRequest</b> header shall be added to the headers collection. If <b>false</b>,
		/// the header shall be removed if previously added.
		/// </remarks>
		public bool IsXmlHttpRequest {
			// For those servers that check, using this header makes us look like a JQuery call:
			// http://www.web-design-talk.co.uk/197/detect-ajax-requests-using-the-x-requested-with-header-and-xmlhttprequest/
			get {
				var value = Headers[CommonHeaders.XRequestedWith];

				return value != null && value == XmlHttpRequestValue;
			}
			set {
				if ( value ) {
					if ( !IsXmlHttpRequest ) {
						Headers.Add( CommonHeaders.XRequestedWith, XmlHttpRequestValue );
					}
				}
				else {
					if ( IsXmlHttpRequest ) {
						Headers.Remove( CommonHeaders.XRequestedWith );
					}
				}
			}
		}
	}
}
