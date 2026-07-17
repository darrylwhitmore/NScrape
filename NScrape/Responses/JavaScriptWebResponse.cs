using System.Net;
using NScrape.Interfaces;

namespace NScrape.Responses;

/// <summary>
/// Represents a web response for a request that returned JavaScript.
/// </summary>
public class JavaScriptWebResponse : TextWebResponse, IJavaScriptWebResponse {

	/// <summary>
	/// Initializes a new instance of the <see cref="JavaScriptWebResponse"/> class.
	/// </summary>
	/// <param name="success"><b>true</b> if the response is considered successful, <b>false</b> otherwise.</param>
	/// <param name="httpWebResponse">The web response object.</param>
	public JavaScriptWebResponse( bool success, HttpWebResponse httpWebResponse )
		: base( success, WebResponseType.JavaScript, httpWebResponse ) {
	}

	/// <summary>
	/// Gets the JavaScript.
	/// </summary>
	public string JavaScript => Text;
}