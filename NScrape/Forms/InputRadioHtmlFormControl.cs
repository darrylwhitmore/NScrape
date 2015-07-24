namespace NScrape.Forms {
	/// <summary>
	/// Represents an HTML <b>input radio</b> control.
	/// </summary>
	public class InputRadioHtmlFormControl : InputCheckableHtmlFormControl {
		/// <summary>
		/// Initializes a new instance of the <see cref="InputRadioHtmlFormControl"/> class.
		/// </summary>
		/// <param name="html">Contains the control HTML.</param>
		public InputRadioHtmlFormControl( string html )
			: base( html ) {
		}
	}
}
