using System;
using System.Drawing;
using System.IO;
using System.Net;

namespace NScrape {
    /// <summary>
	/// Represents a web response for a request that returned an image.
	/// </summary>
    public class ImageWebResponse : WebResponse {
        private Bitmap image;
		private readonly HttpWebResponse webResponse;

	    /// <summary>
	    /// Initializes a new instance of the <see cref="ImageWebResponse"/> class.
	    /// </summary>
	    /// <param name="success"><b>true</b> if the response is considered successful, <b>false</b> otherwise.</param>
	    /// <param name="responseUrl">The URL of the response.</param>
	    /// <param name="image">The image of the response.</param>
		/// <remarks>
		/// Deprecated; please use <see cref="ImageWebResponse( bool, HttpWebResponse )"/> instead.
		/// </remarks>
		[Obsolete( "Please use ImageWebResponse( bool, HttpWebResponse ) instead." )]
		public ImageWebResponse( bool success, Uri responseUrl, Bitmap image )
            : base( success, responseUrl, WebResponseType.Image ) {
            this.image = image;
        }

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
		/// Handles disposal of managed resources.
		/// </summary>
		/// <remarks>
		/// Inheriting classes owning managed resources should override this method and use it to dispose of them.
		/// </remarks>
		protected override void DisposeManagedRessources() {
			base.DisposeManagedRessources();

			if ( webResponse != null ) {
				webResponse.Dispose();
			}
		}

	    /// <summary>
		/// Gets the image.
		/// </summary>
		public Bitmap Image {
			get {
				if ( image == null ) {
					using ( var s = webResponse.GetResponseStream() ) {
						// Skeet says a null is unlikely, but check to make Resharper happy.
						// http://stackoverflow.com/questions/16911056/can-webresponse-getresponsestream-return-a-null
						if ( s == null ) {
							throw new IOException( "HttpWebResponse.GetResponseStream() has returned null" );
						}

						image = new Bitmap( s );
					}
				}

				return image;
			}
		}
    }
}
