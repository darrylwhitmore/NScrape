using System;
using System.Drawing;

namespace NScrape {
    /// <summary>
	/// Represents a web response for a request that returned an image.
	/// </summary>
    public class ImageWebResponse : WebResponse {
        private readonly Bitmap image;

        /// <summary>
		/// Initializes a new instance of the <see cref="ImageWebResponse"/> class.
        /// </summary>
		/// <param name="success"><b>true</b> if the response is considered successful, <b>false</b> otherwise.</param>
		/// <param name="responseUrl">The URL of the response.</param>
		/// <param name="image">The image of the response.</param>
        public ImageWebResponse( bool success, Uri responseUrl, Bitmap image )
            : base( responseUrl, WebResponseType.Image, success ) {
            this.image = image;
        }

        /// <summary>
		/// Gets the image.
		/// </summary>
        public Bitmap Image { get { return image; } }
    }
}
