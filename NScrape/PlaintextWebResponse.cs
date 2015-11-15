using System;
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
		/// <param name="text">The plain text of the response.</param>
		/// <param name="encoding">The encoding of the plain text.</param>
		public PlainTextWebResponse( bool success, Uri responseUrl, string text, Encoding encoding )
            : base( responseUrl, WebResponseType.PlainText, success, text, encoding ) {
        }

        /// <summary>
        /// Gets the plain text.
        /// </summary>
        public string PlainText { get { return Text; } }
    }
}
