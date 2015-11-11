using System;
using System.Text;

namespace NScrape {
    /// <summary>
	/// Represents a web response for a request that returned JavaScript.
	/// </summary>
    public class JavaScriptWebResponse : TextWebResponse {

		public JavaScriptWebResponse( bool success, Uri url, string text, Encoding encoding )
            : base( url, WebResponseType.JavaScript, success, text, encoding ) {
        }

		/// <summary>
		/// Gets the JavaScript.
		/// </summary>
		public string JavaScript { get { return Text; } }
    }
}
