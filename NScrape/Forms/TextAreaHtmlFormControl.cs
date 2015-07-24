using System;
using System.Globalization;
using System.Web;

namespace NScrape.Forms {
	/// <summary>
	/// Represents an HTML <b>textarea</b> control.
	/// </summary>
	public class TextAreaHtmlFormControl : HtmlFormControl {

		internal TextAreaHtmlFormControl( string html ) {
			var match = RegexCache.Instance.Regex( RegexLibrary.ParseTextArea, RegexLibrary.ParseTextAreaOptions ).Match( html );

			if ( match.Success ) {
				AddAttributes( match.Groups[RegexLibrary.ParseTextAreaAttributesGroup].Value );

				// Initalize text if default provided
				Text = match.Groups[RegexLibrary.ParseTextAreaTextGroup].Value;
			}
			else {
				throw new ArgumentException( string.Format( CultureInfo.CurrentCulture, NScrapeResources.NotATextAreaHtmlControl, html ) );
			}
		}

		/// <summary>
		/// Gets the value of the control in <b>application/x-www-form-urlencoded</b> format.
		/// </summary>
		public override string EncodedData {
			get {
				if ( Name.Length > 0 ) {
					return string.Format( CultureInfo.InvariantCulture, "{0}={1}", HttpUtility.UrlEncode( Name ), HttpUtility.UrlEncode( Text ) );
				}

				return string.Empty;
			}
		}

		/// <summary>
		/// Gets or sets the text (value) of the control.
		/// </summary>
		public string Text { get; set; }
	}
}
