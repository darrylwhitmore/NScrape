using System;

namespace NScrape {

    /// <summary>
    /// Represents a GET web request
    /// </summary>
    public class GetWebRequest : WebRequest {

        /// <summary>
		/// Initializes a new instance of the <see cref="GetWebRequest"/> class.
        /// </summary>
        public GetWebRequest()
            : base( WebRequestType.Get ) {
        }

        /// <summary>
		/// Initializes a new instance of the <see cref="GetWebRequest"/> class, specifying the destination.
        /// </summary>
		/// <param name="destination">Specifies the destination of the request.</param>
		public GetWebRequest( Uri destination )
            : base( WebRequestType.Get, destination ) {
        }

        /// <summary>
		/// Initializes a new instance of the <see cref="GetWebRequest"/> class, specifying the destination and redirection.
        /// </summary>
		/// <param name="destination">Specifies the destination of the request.</param>
		/// <param name="autoRedirect"><b>true</b> if the request should be automatically redirected; <b>false</b> otherwise.</param>
		public GetWebRequest( Uri destination, bool autoRedirect )
            : this( destination ) {
            AutoRedirect = autoRedirect;
        }

        /// <summary>
        /// Gets a <see cref="string"/> that represents the current <see cref="GetWebRequest"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents the current <see cref="GetWebRequest"/>.
        /// </returns>
        public override string ToString()
        {
            return $"GET {this.Destination}";
        }
    }
}
