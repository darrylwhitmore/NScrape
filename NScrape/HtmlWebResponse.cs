using System;
using System.Text;

namespace NScrape {
    /// <summary>
	/// Represents a web response for a request that returned HTML.
	/// </summary>
    public class HtmlWebResponse : TextWebResponse {

        public HtmlWebResponse( bool success, Uri url, string html, Encoding encoding )
            : base( url, WebResponseType.Html, success, html, encoding ) {
        }

        /// <summary>
		/// Gets the HTML.
		/// </summary>
        public string Html { get { return Text; } }
    }
}
