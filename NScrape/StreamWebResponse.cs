using System;
using System.IO;
using System.Net;
using NScrape.Interfaces;

namespace NScrape;

/// <summary>
/// Provides the base implementation for classes which represent stream-based web responses.
/// </summary>
public abstract class StreamWebResponse : WebResponse, IStreamWebResponse {
	private readonly HttpWebResponse webResponse;

	/// <summary>
	/// Initializes a new instance of the <see cref="StreamWebResponse"/> class.
	/// </summary>
	/// <param name="success"><b>true</b> if the response is considered successful, <b>false</b> otherwise.</param>
	/// <param name="responseType">The type of the web response.</param>
	/// <param name="webResponse">The web response object.</param>
	protected StreamWebResponse( bool success, WebResponseType responseType, HttpWebResponse webResponse )
		: base( success, webResponse.ResponseUri, responseType ) {
		this.webResponse = webResponse;
	}

	/// <summary>
	/// Gets the length of the content returned by the request.
	/// </summary>
	public long ContentLength => webResponse.ContentLength;

	/// <summary>
	/// Closes the stream-based web response.
	/// </summary>
	/// <remarks>
	/// This method releases the resources associated with the web response, including the underlying stream and connection.
	/// <br/><br/>
	/// It is important to call this method to ensure proper resource management and to avoid connection exhaustion.
	/// Calling this method is equivalent to disposing of the underlying <see cref="HttpWebResponse"/> object.
	/// </remarks>
	public void Close() {
		webResponse?.Dispose();
	}

	/// <summary>
	/// Handles disposal of managed resources.
	/// </summary>
	/// <remarks>
	/// Inheriting classes owning managed resources should override this method and use it to dispose of them.
	/// </remarks>
	protected override void DisposeManagedRessources() {
		base.DisposeManagedRessources();

		webResponse?.Dispose();
	}

	/// <summary>
	/// Retrieves the stream used to read the binary response from the web response.
	/// </summary>
	/// <returns>A <see cref="Stream"/> containing the binary response data.</returns>
	/// <exception cref="InvalidOperationException">
	/// Thrown when the method is called on an instance that was not initialized with a valid <see cref="HttpWebResponse"/> object.
	/// </exception>
	/// <remarks>
	/// This method provides access to the binary data stream of the response. 
	/// It is essential to close the stream after use by calling either <see cref="Stream.Close"/> or <see cref="Close"/> 
	/// to release the connection for reuse. Failure to do so may lead to connection exhaustion in your application.
	/// </remarks>
	/// <seealso cref="Close"/>
	protected Stream GetStream() {
		if ( webResponse != null ) {
			return webResponse.GetResponseStream();
		}

		throw new InvalidOperationException( "This object was not instantiated with a valid HttpWebResponse object." );
	}
}