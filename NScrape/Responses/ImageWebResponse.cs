using System;
using System.IO;
using System.Net;
using NScrape.Interfaces;

namespace NScrape.Responses {
	/// <summary>
	/// Represents a web response for a request that returned an image.
	/// </summary>
    public class ImageWebResponse : StreamWebResponse, IImageWebResponse {
		/// <summary>
		/// Initializes a new instance of the <see cref="ImageWebResponse"/> class.
		/// </summary>
		/// <param name="success"><b>true</b> if the response is considered successful, <b>false</b> otherwise.</param>
		/// <param name="httpWebResponse">The web response object.</param>
		public ImageWebResponse( bool success, HttpWebResponse httpWebResponse )
			: base( success, WebResponseType.Image, httpWebResponse ) {
		}

		/// <summary>
		/// Gets the image stream.
		/// </summary>
		/// <returns>A <see cref="Stream"/> containing the image data.</returns>
		/// <exception cref="InvalidOperationException">
		/// Thrown when the <see cref="HttpWebResponse"/> object is not valid or was not properly instantiated.
		/// </exception>
		/// <remarks>
		/// The <see cref="GetImageStream"/> method provides access to the binary image stream from the response.
		/// <br/><br/>
		/// <note type="important">
		/// Ensure that you call either the <see cref="Stream.Close"/> method or the <see cref="StreamWebResponse.Close"/> method to close the stream and release the connection for reuse.
		/// Failing to close the stream may lead to running out of available connections.
		/// </note>
		/// </remarks>
		/// <seealso cref="StreamWebResponse.Close"/>
		public Stream GetImageStream() {
			return GetStream();
		}
	}
}
