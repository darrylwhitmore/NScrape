namespace NScrape.Forms {
	/// <summary>
	/// Represents a basic HTML form that should do the trick in most cases.
	/// </summary>
	/// <remarks>
	/// If you need additional or specialized functionality, you can create a form class that 
	/// inherits from <see cref="BasicForm"/> or <see cref="HtmlForm"/>.
	/// </remarks>
	public class BasicHtmlForm : BasicForm {
		/// <summary>
		/// Initializes a new instance of the <see cref="BasicHtmlForm"/> class.
		/// </summary>
		/// <param name="webClient">Contains the web client to be used to request and submit the form.</param>
		public BasicHtmlForm( IWebClient webClient )
			: base( webClient ) {
		}

		/// <summary>
		/// Submits the form to the server.
		/// </summary>
		/// <param name="submitButtonName">Contains the <b>name</b> attribute of the submit button or image.</param>
		/// <returns>The server response.</returns>
		/// <remarks>
		/// When the form has multiple submit buttons/images, specify which one should be used; otherwise, for forms with a 
		/// single submit button/image, the parameter may be omitted.
		/// </remarks>
		public WebResponse Submit( string submitButtonName = null ) {
			return SubmitHtmlRequest( BuildRequest( submitButtonName ) );
		}
	}
}
