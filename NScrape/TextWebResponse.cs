using System;
using System.Net;
using System.Text;

namespace NScrape {
    /// <summary>
	/// Provides the base implementation for classes which represent text-based web responses.
	/// </summary>
    public abstract class TextWebResponse : WebResponse {
		private readonly HttpWebResponse webResponse;
        private string text;
        private Encoding encoding;

	    /// <summary>
	    /// Initializes a new instance of the <see cref="TextWebResponse"/> class.
	    /// </summary>
	    /// <param name="success"><b>true</b> if the response is considered successful, <b>false</b> otherwise.</param>
	    /// <param name="responseUrl">The URL of the response.</param>
	    /// <param name="responseType">The type of response.</param>
	    /// <param name="text">The text of the response.</param>
	    /// <param name="encoding">The encoding of the text.</param>
		/// <remarks>
		/// Deprecated; please use <see cref="TextWebResponse( bool, WebResponseType, HttpWebResponse )"/> instead.
		/// </remarks>
		[Obsolete( "Please use TextWebResponse( bool, WebResponseType, HttpWebResponse ) instead." )]
		protected TextWebResponse( bool success, Uri responseUrl, WebResponseType responseType, string text, Encoding encoding )
            : base( success, responseUrl, responseType ) {
            this.text = text;
            this.encoding = encoding;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="TextWebResponse"/> class.
		/// </summary>
		/// <param name="success"><b>true</b> if the response is considered successful, <b>false</b> otherwise.</param>
		/// <param name="responseType">The type of response.</param>
		/// <param name="webResponse">The web response object.</param>
		protected TextWebResponse( bool success, WebResponseType responseType, HttpWebResponse webResponse )
			: base( success, webResponse.ResponseUri, responseType ) {
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
        /// Gets the text.
        /// </summary>
		protected string Text { get { return text ?? ( text = webResponse.GetResponseText() ); } }

		/// <summary>
		/// Gets the encoding of the text.
		/// </summary>
		protected Encoding Encoding { get { return encoding ?? ( encoding = webResponse.GetEncoding() ); } }
    }
}
