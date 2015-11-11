using System;
using System.IO;

namespace NScrape {
    /// <summary>
	/// Represents a web response for a request that was redirected.
	/// </summary>
    public class RedirectedWebResponse : WebResponse {
        private readonly WebRequest request;
        private readonly Uri redirectUrl;

        public RedirectedWebResponse( Uri url, WebRequest request, Uri redirectUrl )
            : base( url, WebResponseType.Redirect, true ) {
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
