using System;
using System.Net;
using System.Text;

namespace NScrape {
    /// <summary>
	/// Represents a web response for a request that returned JavaScript.
	/// </summary>
    public class JavaScriptWebResponse : TextWebResponse {

		/// <summary>
		/// Initializes a new instance of the <see cref="JavaScriptWebResponse"/> class.
		/// </summary>
		/// <param name="success"><b>true</b> if the response is considered successful, <b>false</b> otherwise.</param>
		/// <param name="responseUrl">The URL of the response.</param>
		/// <param name="javaScriptText">The JavaScript text of the response.</param>
		/// <param name="encoding">The encoding of the JavaScript text.</param>
		/// <remarks>
		/// Deprecated; please use <see cref="JavaScriptWebResponse( bool, HttpWebResponse )"/> instead.
		/// </remarks>
		[Obsolete( "Please use JavaScriptWebResponse( bool, HttpWebResponse ) instead." )]
		public JavaScriptWebResponse( bool success, Uri responseUrl, string javaScriptText, Encoding encoding )
            : base( success, responseUrl, WebResponseType.JavaScript, javaScriptText, encoding ) {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="JavaScriptWebResponse"/> class.
		/// </summary>
		/// <param name="success"><b>true</b> if the response is considered successful, <b>false</b> otherwise.</param>
		/// <param name="webResponse">The web response object.</param>
		public JavaScriptWebResponse( bool success, HttpWebResponse webResponse )
			: base( success, WebResponseType.JavaScript, webResponse ) {
		}

		/// <summary>
		/// Gets the JavaScript.
		/// </summary>
		public string JavaScript { get { return Text; } }
    }
}
