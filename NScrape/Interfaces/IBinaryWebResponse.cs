using System.IO;

namespace NScrape.Interfaces;

/// <summary>
/// Defines the contract for a binary web response, which provides access to the binary data 
/// returned from a web request.
/// </summary>
/// <remarks>
/// This interface extends <see cref="NScrape.Interfaces.IStreamWebResponse"/> to include 
/// functionality specific to binary data handling. Implementations of this interface should 
/// ensure proper management of the underlying stream to avoid resource leaks or connection 
/// exhaustion.
/// </remarks>
/// <seealso cref="NScrape.Interfaces.IStreamWebResponse"/>
public interface IBinaryWebResponse : IStreamWebResponse {
	/// <summary>
	/// Gets the stream that is used to read the binary response.
	/// </summary>
	/// <returns>A <see cref="Stream"/> containing the binary response.</returns>
	Stream GetResponseStream();
}