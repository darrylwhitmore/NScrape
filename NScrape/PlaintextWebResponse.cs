using System;
using System.Text;

namespace NScrape {
    /// <summary>
	/// Represents a web response for a request that returned plain text.
	/// </summary>
    public class PlainTextWebResponse : TextWebResponse {

        public PlainTextWebResponse( bool success, Uri url, string text, Encoding encoding )
            : base( url, WebResponseType.PlainText, success, text, encoding ) {
        }

        /// <summary>
        /// Gets the plain text.
        /// </summary>
        public string PlainText { get { return Text; } }
    }
}
