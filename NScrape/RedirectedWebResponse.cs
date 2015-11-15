using System;
using System.IO;

namespace NScrape {
    /// <summary>
	/// Represents a web response for a request that was redirected.
	/// </summary>
    public class RedirectedWebResponse : WebResponse {
        private readonly WebRequest request;
        private readonly Uri redirectUrl;

        /// <summary>
		/// Initializes a new instance of the <see cref="RedirectedWebResponse"/> class.
        /// </summary>
		/// <param name="responseUrl">The URL of the response.</param>
        /// <param name="request">The original web request.</param>
		/// <param name="redirectUrl">The redirect URL of the response.</param>
        public RedirectedWebResponse( Uri responseUrl, WebRequest request, Uri redirectUrl )
            : base( responseUrl, WebResponseType.Redirect, true ) {
            this.request = request;
            this.redirectUrl = redirectUrl;
        }

        /// <summary>
        /// Gets the redirect URL
        /// </summary>
        public Uri RedirectUrl { get { return redirectUrl; } }

        /// <summary>
        /// Gets the original web request.
        /// </summary>
        public WebRequest WebRequest { get { return request; } }
    }
}
