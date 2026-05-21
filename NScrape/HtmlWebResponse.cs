using System;
using System.Net;
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
		/// <param name="webResponse">The web response object.</param>
		public HtmlWebResponse( bool success, HttpWebResponse webResponse )
			: base( success, WebResponseType.Html, webResponse ) {
		}

        /// <summary>
		/// Gets the HTML.
		/// </summary>
        public string Html { get { return Text; } }
    }
}
