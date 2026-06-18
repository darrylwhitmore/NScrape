using System.Net;
using NScrape.Interfaces;

namespace NScrape.Responses;

/// <summary>
/// Represents a web response for a request that returned JSON.
/// </summary>
public class JsonWebResponse : TextWebResponse, IJsonWebResponse {

	/// <summary>
	/// Initializes a new instance of the <see cref="JsonWebResponse"/> class.
	/// </summary>
	/// <param name="success"><b>true</b> if the response is considered successful, <b>false</b> otherwise.</param>
	/// <param name="httpWebResponse">The web response object.</param>
	public JsonWebResponse( bool success, HttpWebResponse httpWebResponse )
		: base( success, WebResponseType.Json, httpWebResponse ) {
	}

	/// <summary>
	/// Gets the JSON data.
	/// </summary>
	public string Json => Text;
}