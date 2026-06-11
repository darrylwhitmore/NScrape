using System;
using System.IO;
using System.Net;
using NScrape.Interfaces;

namespace NScrape.Responses {
	/// <summary>
	/// Represents a web response for a request that returned binary data.
	/// </summary>
    public class BinaryWebResponse : WebResponse, IBinaryWebResponse {
	    private readonly HttpWebResponse httpWebResponse;

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryWebResponse"/> class.
		/// </summary>
		/// <param name="success"><b>true</b> if the response is considered successful, <b>false</b> otherwise.</param>
		/// <param name="httpWebResponse">The web response object.</param>
		public BinaryWebResponse( bool success, HttpWebResponse httpWebResponse )
			: base( success, httpWebResponse.ResponseUri, WebResponseType.Binary ) {
			this.httpWebResponse = httpWebResponse;
		}
		
		/// <summary>
		/// Gets the length of the content returned by the request.
		/// </summary>
		public long ContentLength => httpWebResponse.ContentLength;

		/// <summary>
		/// Closes the binary response stream and releases associated resources.
		/// </summary>
		/// <remarks>
		/// This method ensures proper resource management by releasing the underlying stream and connection.
		/// It is equivalent to disposing of the underlying <see cref="HttpWebResponse"/> object.
		/// Failure to call this method may result in resource leaks or connection exhaustion.
		/// </remarks>
		public void Close() {
		    httpWebResponse?.Dispose();
	    }
		
	    /// <summary>
	    /// Handles disposal of resources.
	    /// </summary>
	    /// <remarks>
	    /// Inheriting classes owning disposable resources should override this method and use it to dispose of them.
	    /// </remarks>
	    protected override void DisposeResources() {
		    base.DisposeResources();

		    httpWebResponse?.Dispose();
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
	    /// It is essential to close the stream after use by calling either <see cref="Stream.Close"/> or <see cref="Close"/> 
	    /// to release the connection for reuse. Failure to do so may lead to connection exhaustion in your application.
	    /// </remarks>
	    /// <seealso cref="Close"/>
		public Stream GetStream() {
			if ( httpWebResponse != null ) {
				return httpWebResponse.GetResponseStream();
			}

			throw new InvalidOperationException( "This object was not instantiated with a valid HttpWebResponse object." );
		}
    }
}
