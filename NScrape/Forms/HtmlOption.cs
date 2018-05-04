using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace NScrape.Forms {

	/// <summary>
	/// Represents an HTML <b>select</b> control <b>option</b>.
	/// </summary>
	public class HtmlOption {
		private readonly Dictionary<string, string> attributes = new Dictionary<string, string>( StringComparer.OrdinalIgnoreCase );

		internal HtmlOption( string html ) {
			var match = RegexCache.Instance.Regex( RegexLibrary.ParseOption, RegexLibrary.ParseOptionOptions ).Match( html );

			if ( match.Success ) {
				Option = match.Groups[RegexLibrary.ParseOptionOptionGroup].Value;

				foreach ( var attribute in Utility.ParseAttributes( match.Groups[RegexLibrary.ParseOptionAttributesGroup].Value ) ) {
					attributes.Add( attribute.Key, attribute.Value );
				}

				if ( Attributes.ContainsKey( "value" ) ) {
					Value = Attributes["value"];
				}

				if ( Attributes.ContainsKey( "selected" ) ) {
					Selected = true;
				}
			}
			else {
				throw new ArgumentException( string.Format( CultureInfo.CurrentCulture, Properties.Resources.NotASelectHtmlControlOption, html ) );
			}
		}

		/// <summary>
		/// Gets the attributes of the option.
		/// </summary>
		public ReadOnlyDictionary<string, string> Attributes { get { return new ReadOnlyDictionary<string, string>( attributes ); } }

		/// <summary>
		/// Gets the option text.
		/// </summary>
		public string Option { get; private set; }

		/// <summary>
		/// Gets or sets whether or not the option is selected.
		/// </summary>
		public bool Selected { get; set; }

		/// <summary>
		/// Gets the value of the option.
		/// </summary>
		public string Value { get; private set; }
	}
}
