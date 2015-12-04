using System;
using System.Text;

namespace NScrape {
	/// <summary>
	/// Represents a web response for a request that returned JSON.
	/// </summary>
	public class JsonWebResponse : TextWebResponse {

		/// <summary>
		/// Initializes a new instance of the <see cref="JsonWebResponse"/> class.
		/// </summary>
		/// <param name="success"><b>true</b> if the response is considered successful, <b>false</b> otherwise.</param>
		/// <param name="responseUrl">The URL of the response.</param>
		/// <param name="jsonText">The JSON text of the response.</param>
		/// <param name="encoding">The encoding of the JSON text.</param>
		public JsonWebResponse( bool success, Uri responseUrl, string jsonText, Encoding encoding )
			: base( success, responseUrl, WebResponseType.Json, jsonText, encoding ) {
		}

		/// <summary>
		/// Gets the JSON data.
		/// </summary>
		public string Json { get { return Text; } }
	}
}
