using System;

namespace NScrape {

    /// <summary>
	/// Represents a POST web request
	/// </summary>
    public class PostWebRequest : WebRequest {
        private string contentType = "application/x-www-form-urlencoded";
        private string requestData = string.Empty;

        /// <summary>
		/// Initializes a new instance of the <see cref="PostWebRequest"/> class.
		/// </summary>
        public PostWebRequest()
            : base( WebRequestType.Post ) {
        }

        /// <summary>
		/// Initializes a new instance of the <see cref="PostWebRequest"/> class, specifying the destination and request data.
		/// </summary>
		/// <param name="destination">Specifies the destination of the request.</param>
		/// <param name="requestData">Contains the request data in <b>application/x-www-form-urlencoded</b> format.</param>
		/// <remarks>
		/// Format the request data per the request's content type (by default, <b>application/x-www-form-urlencoded</b>).
		/// </remarks>
		/// <example>
		/// <code>
		/// var destination = new Uri( "http://www.foo.com" );
		/// var requestData = "step=confirmation&amp;rt=L&amp;rp=%2Flogin%2Fhome&amp;p=0&amp;inputEmailHandle=foo&amp;inputPassword=bar";
		/// 
		/// var request = new PostWebRequest( destination, requestData );
		/// </code>
		/// </example>
		public PostWebRequest( Uri destination, string requestData )
            : base( WebRequestType.Post, destination ) {
            this.requestData = requestData;
        }

        /// <summary>
		/// Initializes a new instance of the <see cref="PostWebRequest"/> class, specifying the destination, request data and redirection.
		/// </summary>
		/// <param name="destination">Specifies the destination of the request.</param>
		/// <param name="requestData">Contains the request data in <b>application/x-www-form-urlencoded</b> format.</param>
		/// <param name="autoRedirect"><b>true</b> if the request should be automatically redirected; <b>false</b> otherwise.</param>
		/// <remarks>
		/// Format the request data per the request's content type (by default, <b>application/x-www-form-urlencoded</b>).
		/// </remarks>
		/// <example>
		/// <code>
		/// var destination = new Uri( "http://www.foo.com" );
		/// var requestData = "step=confirmation&amp;rt=L&amp;rp=%2Flogin%2Fhome&amp;p=0&amp;inputEmailHandle=foo&amp;inputPassword=bar";
		/// var autoRedirect = false;
		/// 
		/// var request = new PostWebRequest( destination, requestData, autoRedirect );
		/// </code>
		/// </example>
		public PostWebRequest( Uri destination, string requestData, bool autoRedirect )
            : this( destination, requestData ) {
            AutoRedirect = autoRedirect;
        }

        /// <summary>
		/// Initializes a new instance of the <see cref="PostWebRequest"/> class, specifying the destination, request data and content type.
		/// </summary>
		/// <param name="destination">Specifies the destination of the request.</param>
		/// <param name="requestData">Contains the request data.</param>
		/// <param name="contentType">Contains the <b>MIME</b> content type.</param>
		/// <remarks>
		/// Format the request data per the specified <b>MIME</b> content type.
		/// </remarks>
		/// <example>
		/// <code>
		///	var request = new PostWebRequest( new Uri( "http://www.foo.com" ), "data data data", "text/plain" );
		/// </code>
		/// </example>
		public PostWebRequest( Uri destination, string requestData, string contentType )
            : this( destination, requestData ) {
            this.contentType = contentType;
        }

        /// <summary>
		/// Initializes a new instance of the <see cref="PostWebRequest"/> class, specifying the destination, request data, content type and redirection.
		/// </summary>
		/// <param name="destination">Specifies the destination of the request.</param>
		/// <param name="requestData">Contains the request data.</param>
		/// <param name="contentType">Contains the <b>MIME</b> content type.</param>
		/// <param name="autoRedirect"><b>true</b> if the request should be automatically redirected; <b>false</b> otherwise.</param>
		/// <remarks>
		/// Format the request data per the specified <b>MIME</b> content type.
		/// </remarks>
		/// <example>
		/// <code>
		///	var request = new PostWebRequest( new Uri( "http://www.foo.com" ), "data data data", "text/plain", false );
		/// </code>
		/// </example>
		public PostWebRequest( Uri destination, string requestData, string contentType, bool autoRedirect )
            : this( destination, requestData, contentType ) {
            AutoRedirect = autoRedirect;
        }

        /// <summary>
        /// Gets or sets the content type.
        /// </summary>
        /// <remarks>
		/// By default, the content type is set to <b>application/x-www-form-urlencoded</b>.
        /// </remarks>
        public string ContentType { get { return contentType; } set { contentType = value; } }

        /// <summary>
        /// Gets or sets the request data.
        /// </summary>
        public string RequestData { get { return requestData; } set { requestData = value; } }

        /// <summary>
        /// Gets a <see cref="string"/> that represents the current <see cref="PostWebRequest"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents the current <see cref="PostWebRequest"/>.
        /// </returns>
        public override string ToString()
        {
            return $"POST {this.Destination}";
        }
    }
}
