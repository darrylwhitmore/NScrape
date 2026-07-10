using System;
using System.IO;
using System.Net;
using NScrape.Interfaces;

namespace NScrape.Responses;

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
	/// Retrieves the stream containing the binary data of the web response.
	/// </summary>
	/// <returns>
	/// A <see cref="Stream"/> that provides access to the binary data of the response.
	/// </returns>
	/// <exception cref="InvalidOperationException">
	/// Thrown if the method is invoked on an instance that was not initialized with a valid 
	/// <see cref="HttpWebResponse"/> object.
	/// </exception>
	/// <remarks>
	/// This method allows access to the binary data stream of the response. Be sure to 
	/// properly dispose of the stream after use to prevent resource leaks and connection exhaustion.
	/// </remarks>
	public Stream GetStream() {
		if ( httpWebResponse != null ) {
			return httpWebResponse.GetResponseStream();
		}

		throw new InvalidOperationException( "This object was not instantiated with a valid HttpWebResponse object." );
	}
}
