using System.Net;

namespace NScrape.Interfaces;

/// <summary>
/// Defines the contract for a stream-based web response.
/// </summary>
public interface IStreamWebResponse : IWebResponse {
	/// <summary>
	/// Gets the length of the content returned by the request.
	/// </summary>
	long ContentLength { get; }

	/// <summary>
	/// Closes the stream-based web response.
	/// </summary>
	/// <remarks>
	/// This method releases the resources associated with the web response, including the underlying stream and connection.
	/// <br/><br/>
	/// It is important to call this method to ensure proper resource management and to avoid connection exhaustion.
	/// Calling this method is equivalent to disposing of the underlying <see cref="HttpWebResponse"/> object.
	/// </remarks>
	void Close();
}