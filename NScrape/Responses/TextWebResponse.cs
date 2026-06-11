using System.Net;
using System.Text;

namespace NScrape.Responses {
    /// <summary>
	/// Provides the base implementation for classes which represent text-based web responses.
	/// </summary>
    public abstract class TextWebResponse : WebResponse {
		private readonly HttpWebResponse httpWebResponse;
        private string text;
        private Encoding encoding;

		/// <summary>
		/// Initializes a new instance of the <see cref="TextWebResponse"/> class.
		/// </summary>
		/// <param name="success"><b>true</b> if the response is considered successful, <b>false</b> otherwise.</param>
		/// <param name="responseType">The type of response.</param>
		/// <param name="httpWebResponse">The web response object.</param>
		protected TextWebResponse( bool success, WebResponseType responseType, HttpWebResponse httpWebResponse )
			: base( success, httpWebResponse.ResponseUri, responseType ) {
			this.httpWebResponse = httpWebResponse;
		}

		/// <summary>
		/// Handles disposal of resources.
		/// </summary>
		/// <remarks>
		/// Inheriting classes owning disposable resources should override this method and use it to dispose of them.
		/// </remarks>
		protected override void DisposeResources() {
			base.DisposeResources();

			httpWebResponse?.Dispose();
		}

	    /// <summary>
        /// Gets the text.
        /// </summary>
		protected string Text { get { return text ?? ( text = httpWebResponse.GetResponseText() ); } }

		/// <summary>
		/// Gets the encoding of the text.
		/// </summary>
		protected Encoding Encoding { get { return encoding ?? ( encoding = httpWebResponse.GetEncoding() ); } }
    }
}
