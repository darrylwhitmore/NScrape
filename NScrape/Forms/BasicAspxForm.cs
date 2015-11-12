namespace NScrape.Forms {
	/// <summary>
	/// Represents a basic ASPX form that should do the trick in most cases.
	/// </summary>
	/// <remarks>
	/// If you need additional or specialized functionality, you can create a form class that 
	/// inherits from <see cref="BasicForm"/> or <see cref="HtmlForm"/>.
	/// </remarks>
	public class BasicAspxForm : BasicForm {
		/// <summary>
		/// Initializes a new instance of the <see cref="BasicAspxForm"/> class.
		/// </summary>
		/// <param name="webClient">Contains the web client to be used to request and submit the form.</param>
		public BasicAspxForm( IWebClient webClient )
			: base( webClient ) {
		}

		/// <summary>
		/// Submits the form to the server.
		/// </summary>
		/// <param name="eventTargetValue">Contains the value for <b>__EVENTTARGET</b>, the control doing the submission.</param>
		/// <param name="eventArgumentValue">Contains the value for <b>__EVENTARGUMENT</b>, any additional information.</param>
		/// <returns>The server response.</returns>
		public WebResponse Submit( string eventTargetValue, string eventArgumentValue = "" ) {
			return SubmitHtmlRequest( BuildAspxPostBackRequest( eventTargetValue, eventArgumentValue ) );
		}
	}
}
