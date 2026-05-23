using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.Versioning;

namespace NScrape {
    /// <summary>
	/// Represents a web response for a request that returned an image.
	/// </summary>
    public class ImageWebResponse : WebResponse {
        private Bitmap cachedBitmap;
		private readonly HttpWebResponse webResponse;

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageWebResponse"/> class.
		/// </summary>
		/// <param name="success"><b>true</b> if the response is considered successful, <b>false</b> otherwise.</param>
		/// <param name="webResponse">The web response object.</param>
		public ImageWebResponse( bool success, HttpWebResponse webResponse )
			: base( success, webResponse.ResponseUri, WebResponseType.Image ) {
			this.webResponse = webResponse;
		}

		/// <summary>
		/// Gets the length of the content returned by the request.
		/// </summary>
		public long ContentLength => webResponse.ContentLength;

		/// <summary>
		/// Closes the image response stream.
		/// </summary>
		/// <remarks>
		/// The Close method closes the image response stream and releases the connection to the resource for reuse by other requests.
		/// <br/><br/>
		/// You must call either the <see cref="Stream.Close">Stream.Close</see> or the <see cref="ImageWebResponse.Close"/> method to close the stream and release the
		/// connection for reuse. It is not necessary to call both <see cref="Stream.Close">Stream.Close</see> and <see cref="ImageWebResponse.Close"/>, but doing so does not cause an
		/// error. Failure to close the stream can cause your application to run out of connections.
		/// </remarks>
		/// <seealso cref="GetImageStream"/>
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
					using ( var s = webResponse.GetResponseStream() ) {
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
		/// <remarks>
		/// The GetResponseStream method returns the binary image stream from the response.
		/// <br/><br/>
		/// <note>
		/// You must call either the <see cref="Stream.Close">Stream.Close</see> or the <see cref="ImageWebResponse.Close"/> method to close the stream and release the
		/// connection for reuse. It is not necessary to call both <see cref="Stream.Close">Stream.Close</see> and <see cref="ImageWebResponse.Close"/>, but doing so does not cause an
		/// error. Failure to close the stream can cause your application to run out of connections.
		/// </note>
		/// </remarks>
		/// <seealso cref="Close"/>
		public Stream GetImageStream() {
			if ( webResponse != null ) {
				return webResponse.GetResponseStream();
			}

			throw new InvalidOperationException( "This object was not instantiated with a valid HttpWebResponse object." );
		}
	}
}
