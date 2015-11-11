using System;
using System.Drawing;

namespace NScrape {
    /// <summary>
	/// Represents a web response for a request that returned an image.
	/// </summary>
    public class ImageWebResponse : WebResponse {
        private readonly Bitmap image;

        public ImageWebResponse( bool success, Uri url, Bitmap image )
            : base( url, WebResponseType.Image, success ) {
            this.image = image;
        }

        /// <summary>
		/// Gets the image.
		/// </summary>
        public Bitmap Image { get { return image; } }
    }
}
