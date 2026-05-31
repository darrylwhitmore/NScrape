using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.Versioning;
using NScrape.Interfaces;

namespace NScrape.Responses {
	/// <summary>
	/// Represents a web response for a request that returned an image.
	/// </summary>
    public class ImageWebResponse : StreamWebResponse, IImageWebResponse {
        private Bitmap cachedBitmap;

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageWebResponse"/> class.
		/// </summary>
		/// <param name="success"><b>true</b> if the response is considered successful, <b>false</b> otherwise.</param>
		/// <param name="httpWebResponse">The web response object.</param>
		public ImageWebResponse( bool success, HttpWebResponse httpWebResponse )
			: base( success, WebResponseType.Image, httpWebResponse ) {
		}

		/// <summary>
		/// Gets the image.
		/// </summary>
		/// <remarks>
		/// Deprecated; please use <see cref="GetImageStream()"/> instead.
		/// </remarks>
		[SupportedOSPlatform( "windows6.1" )]
		[Obsolete( "Please use GetImageStream() instead." )]
		public Bitmap Image {
			get {
				if ( cachedBitmap == null ) {
					using ( var s = GetStream() ) {
						// Skeet says a null is unlikely, but check to make Resharper happy.
						// http://stackoverflow.com/questions/16911056/can-webresponse-getresponsestream-return-a-null
						if ( s == null ) {
							throw new IOException( "HttpWebResponse.GetResponseStream() has returned null" );
						}

						cachedBitmap = new Bitmap( s );
					}
				}

				return cachedBitmap;
			}
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
