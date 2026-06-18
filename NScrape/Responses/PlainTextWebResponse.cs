using System.Net;
using NScrape.Interfaces;

namespace NScrape.Responses;

/// <summary>
/// Represents a web response for a request that returned plain text.
/// </summary>
public class PlainTextWebResponse : TextWebResponse, IPlainTextWebResponse {

	/// <summary>
	/// Initializes a new instance of the <see cref="PlainTextWebResponse"/> class.
	/// </summary>
	/// <param name="success"><b>true</b> if the response is considered successful, <b>false</b> otherwise.</param>
	/// <param name="httpWebResponse">The web response object.</param>
	public PlainTextWebResponse( bool success, HttpWebResponse httpWebResponse )
		: base( success, WebResponseType.PlainText, httpWebResponse ) {
	}

	/// <summary>
	/// Gets the plain text.
	/// </summary>
	public string PlainText => Text;
}