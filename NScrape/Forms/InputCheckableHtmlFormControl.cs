namespace NScrape.Forms {
	/// <summary>
	/// Provides the base implementation for HTML controls that may be <i>checked</i>.
	/// </summary>
	public abstract class InputCheckableHtmlFormControl : InputHtmlFormControl {

		internal InputCheckableHtmlFormControl( string html )
			: base( html ) {

			Checked = ( Attributes.ContainsKey( "checked" ) );
		}

		/// <summary>
		/// Gets or sets whether or not the control is checked.
		/// </summary>
		public bool Checked { get; set; }

		/// <summary>
		/// Gets the value of the control.
		/// </summary>
		public new string Value { get { return base.Value; } }
	}
}