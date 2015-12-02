using System;
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
		public JavaScriptWebResponse( bool success, Uri responseUrl, string javaScriptText, Encoding encoding )
            : base( success, responseUrl, WebResponseType.JavaScript, javaScriptText, encoding ) {
        }

		/// <summary>
		/// Gets the JavaScript.
		/// </summary>
		public string JavaScript { get { return Text; } }
    }
}
