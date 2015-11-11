using System;
using System.Text;

namespace NScrape {
    /// <summary>
	/// Provides the base implementation for classes which represent text-based web responses.
	/// </summary>
    public abstract class TextWebResponse : WebResponse {
        private readonly string text;
        private readonly Encoding encoding;

        /// <summary>
		/// Initializes a new instance of the <see cref="TextWebResponse"/> class.
		/// </summary>
		/// <param name="responseUrl">The URL of the response.</param>
		/// <param name="responseType">The type of response.</param>
		/// <param name="success"><b>true</b> if the response is considered successful, <b>false</b> otherwise.</param>
		/// <param name="text">The text of the response.</param>
        /// <param name="encoding">The encoding of the text.</param>
        public TextWebResponse( Uri responseUrl, WebResponseType responseType, bool success, string text, Encoding encoding )
            : base( responseUrl, responseType, success ) {
            this.text = text;
            this.encoding = encoding;
        }

        /// <summary>
        /// Gets the text.
        /// </summary>
        protected string Text { get { return text; } }

        /// <summary>
        /// Gets the encoding of the text.
        /// </summary>
        protected Encoding Encoding { get { return encoding; } }
    }
}
