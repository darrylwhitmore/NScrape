using System;
using System.IO;
using System.Net;
using NScrape.Interfaces;

namespace NScrape {
	/// <summary>
	/// Represents a web response for a request that returned binary data.
	/// </summary>
    public class BinaryWebResponse : StreamWebResponse, IBinaryWebResponse {
	    /// <summary>
	    /// Initializes a new instance of the <see cref="BinaryWebResponse"/> class.
	    /// </summary>
		/// <param name="success"><b>true</b> if the response is considered successful, <b>false</b> otherwise.</param>
		/// <param name="webResponse">The web response object.</param>
	    public BinaryWebResponse( bool success, HttpWebResponse webResponse )
			: base( success, WebResponseType.Binary, webResponse ) {
	    }

		/// <summary>
		/// Gets the stream that is used to read the binary response.
		/// </summary>
		/// <returns>A <see cref="Stream"/> containing the binary response.</returns>
		/// <exception cref="InvalidOperationException">
		/// Thrown when the method is called on an instance that was not initialized with a valid <see cref="HttpWebResponse"/> object.
		/// </exception>
		/// <remarks>
		/// This method provides access to the binary data stream of the response. 
		/// It is essential to close the stream after use by calling either <see cref="Stream.Close"/> or <see cref="StreamWebResponse.Close"/> 
		/// to release the connection for reuse. Failure to do so may lead to connection exhaustion in your application.
		/// </remarks>
		/// <seealso cref="StreamWebResponse.Close"/>
		public Stream GetResponseStream() {
			return GetStream();
		}
	}
}
