using System;
using System.IO;
using System.Net;
using NScrape.Responses;

namespace NScrape.Interfaces;

/// <summary>
/// Defines the contract for a web response that provides access to image data.
/// </summary>
/// <remarks>
/// This interface extends the <see cref="NScrape.Interfaces.IStreamWebResponse"/> interface to include functionality specific to image-based web responses.
/// Implementations of this interface should provide access to the image data as a <see cref="System.IO.Stream"/>.
/// </remarks>
/// <seealso cref="NScrape.Interfaces.IStreamWebResponse"/>
public interface IImageWebResponse : IStreamWebResponse {
	/// <summary>
	/// Gets the image stream.
	/// </summary>
	/// <returns>A <see cref="Stream"/> containing the image data.</returns>
	/// <exception cref="InvalidOperationException">
	/// Thrown when the <see cref="HttpWebResponse"/> object is not valid or was not properly instantiated.
	/// </exception>
	/// <remarks>
	/// The <see cref="IImageWebResponse.GetImageStream"/> method provides access to the binary image stream from the response.
	/// <br/><br/>
	/// <note type="important">
	/// Ensure that you call either the <see cref="Stream.Close"/> method or the <see cref="StreamWebResponse.Close"/> method to close the stream and release the connection for reuse.
	/// Failing to close the stream may lead to running out of available connections.
	/// </note>
	/// </remarks>
	/// <seealso cref="StreamWebResponse.Close"/>
	Stream GetImageStream();
}