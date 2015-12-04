using System;

namespace NScrape {
    /// <summary>
	/// Represents a web response for a request that returned unsupported content.
	/// </summary>
    public class UnsupportedWebResponse : WebResponse {
        private readonly string contentType;

	    /// <summary>
	    /// Initializes a new instance of the <see cref="UnsupportedWebResponse"/> class.
	    /// </summary>
	    /// <param name="responseUrl">The URL of the response.</param>
	    /// <param name="contentType">Contains the <b>MIME</b> content type.</param>
	    public UnsupportedWebResponse( Uri responseUrl, string contentType )
            : base( false, responseUrl, WebResponseType.Unsupported ) {
            this.contentType = contentType;
        }

        /// <summary>
		/// Gets the content type.
		/// </summary>
        public string ContentType { get { return contentType; } }
    }
}
