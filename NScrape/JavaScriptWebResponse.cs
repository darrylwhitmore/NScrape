using System.Net;
using NScrape.Interfaces;

namespace NScrape {
	/// <summary>
	/// Represents a web response for a request that returned JavaScript.
	/// </summary>
    public class JavaScriptWebResponse : TextWebResponse, IJavaScriptWebResponse {

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
