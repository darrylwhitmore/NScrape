using System;
using System.Net;

namespace NScrape {
    /// <summary>
	/// Represents a web response for a request that returned an exception.
	/// </summary>
    public class ExceptionWebResponse : WebResponse {
        private readonly WebException exception;

        public ExceptionWebResponse( WebException exception, Uri url )
            : base( url, WebResponseType.Exception, false ) {
            this.exception = exception;
        }

        /// <summary>
		/// Gets the exception.
		/// </summary>
        public WebException Exception { get { return exception; } }
    }
}
