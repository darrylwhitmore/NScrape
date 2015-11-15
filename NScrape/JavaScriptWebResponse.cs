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
		/// <param name="text">The JavaScript text of the response.</param>
		/// <param name="encoding">The encoding of the JavaScript text.</param>
		public JavaScriptWebResponse( bool success, Uri responseUrl, string text, Encoding encoding )
            : base( responseUrl, WebResponseType.JavaScript, success, text, encoding ) {
        }

		/// <summary>
		/// Gets the JavaScript.
		/// </summary>
		public string JavaScript { get { return Text; } }
    }
}
