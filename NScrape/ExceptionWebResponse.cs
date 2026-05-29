using System;
using System.Net;
using NScrape.Interfaces;

namespace NScrape {
	/// <summary>
	/// Represents a web response for a request that returned an exception.
	/// </summary>
    public class ExceptionWebResponse : WebResponse, IExceptionWebResponse {
        private readonly WebException exception;

	    /// <summary>
	    /// Initializes a new instance of the <see cref="ExceptionWebResponse"/> class.
	    /// </summary>
	    /// <param name="responseUrl">The URL of the response.</param>
	    /// <param name="exception">The exception of the response.</param>
	    public ExceptionWebResponse( Uri responseUrl, WebException exception )
            : base( false, responseUrl, WebResponseType.Exception ) {
            this.exception = exception;
        }

        /// <summary>
		/// Gets the exception.
		/// </summary>
        public WebException Exception { get { return exception; } }
    }
}
