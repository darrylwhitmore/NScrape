using System;
using System.Net;
using System.Text;

namespace NScrape {
    /// <summary>
	/// Represents a web response for a request that returned plain text.
	/// </summary>
    public class PlainTextWebResponse : TextWebResponse {

		/// <summary>
		/// Initializes a new instance of the <see cref="PlainTextWebResponse"/> class.
		/// </summary>
		/// <param name="success"><b>true</b> if the response is considered successful, <b>false</b> otherwise.</param>
		/// <param name="responseUrl">The URL of the response.</param>
		/// <param name="plainText">The plain text of the response.</param>
		/// <param name="encoding">The encoding of the plain text.</param>
		[Obsolete( "PlainTextWebResponse( bool, Uri, string, Encoding ) is deprecated, please use PlainTextWebResponse( bool, HttpWebResponse ) instead." )]
		public PlainTextWebResponse( bool success, Uri responseUrl, string plainText, Encoding encoding )
            : base( success, responseUrl, WebResponseType.PlainText, plainText, encoding ) {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="PlainTextWebResponse"/> class.
		/// </summary>
		/// <param name="success"><b>true</b> if the response is considered successful, <b>false</b> otherwise.</param>
		/// <param name="webResponse">The web response object.</param>
		public PlainTextWebResponse( bool success, HttpWebResponse webResponse )
			: base( success, WebResponseType.PlainText, webResponse ) {
		}

        /// <summary>
        /// Gets the plain text.
        /// </summary>
        public string PlainText { get { return Text; } }
    }
}
