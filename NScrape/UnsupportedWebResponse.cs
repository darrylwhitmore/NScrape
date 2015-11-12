using System;

namespace NScrape {
    /// <summary>
	/// Represents a web response for a request that returned unsupported content.
	/// </summary>
    public class UnsupportedWebResponse : WebResponse {
        private readonly string contentType;

        public UnsupportedWebResponse( string contentType, Uri responseUrl )
            : base( responseUrl, WebResponseType.Unsupported, false ) {
            this.contentType = contentType;
        }

        /// <summary>
		/// Gets the content type.
		/// </summary>
        public string ContentType { get { return contentType; } }
    }
}
