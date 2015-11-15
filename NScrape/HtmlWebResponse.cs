using System;
using System.Text;

namespace NScrape {
    /// <summary>
	/// Represents a web response for a request that returned HTML.
	/// </summary>
    public class HtmlWebResponse : TextWebResponse {

		/// <summary>
		/// Initializes a new instance of the <see cref="HtmlWebResponse"/> class.
		/// </summary>
		/// <param name="success"><b>true</b> if the response is considered successful, <b>false</b> otherwise.</param>
		/// <param name="responseUrl">The URL of the response.</param>
		/// <param name="html">The HTML text of the response.</param>
		/// <param name="encoding">The encoding of the HTML text.</param>
		public HtmlWebResponse( bool success, Uri responseUrl, string html, Encoding encoding )
            : base( responseUrl, WebResponseType.Html, success, html, encoding ) {
        }

        /// <summary>
		/// Gets the HTML.
		/// </summary>
        public string Html { get { return Text; } }
    }
}
