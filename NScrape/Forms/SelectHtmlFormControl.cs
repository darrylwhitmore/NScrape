using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace NScrape.Forms {

	/// <summary>
	/// Represents an HTML <b>select</b> control.
	/// </summary>
	public class SelectHtmlFormControl : HtmlFormControl {

		internal SelectHtmlFormControl( string html ) {
			var match = RegexCache.Instance.Regex( RegexLibrary.ParseSelect, RegexLibrary.ParseSelectOptions ).Match( html );

			if ( match.Success ) {
				AddAttributes( match.Groups[RegexLibrary.ParseSelectAttributesGroup].Value );

				Options = BuildOptionList( match.Groups[RegexLibrary.ParseSelectOptionsGroup].Value );
			}
			else {
				throw new ArgumentException( string.Format( CultureInfo.CurrentCulture, NScrapeResources.NotASelectHtmlControl, html ) );
			}
		}

		private static ReadOnlyCollection<HtmlOption> BuildOptionList( string optionListHtml ) {
			var options = new List<HtmlOption>();

			var matches = RegexCache.Instance.Regex( RegexLibrary.ParseOptionList, RegexLibrary.ParseOptionListOptions ).Matches( optionListHtml );

			foreach ( Match match in matches ) {
				options.Add( new HtmlOption( match.Value ) );
			}

			return new ReadOnlyCollection<HtmlOption>( options );
		}

		/// <summary>
		/// Gets the value of the control in <b>application/x-www-form-urlencoded</b> format.
		/// </summary>
		public override string EncodedData {
			get {
				if ( Name.Length > 0 ) {
					var sb = new StringBuilder();

					foreach ( var option in Options ) {
						if ( option.Selected ) {
							if ( sb.Length > 0 ) {
								sb.Append( "&" );
							}

							sb.AppendFormat( "{0}=", HttpUtility.UrlEncode( Name ) );

							if ( option.Value.Length > 0 ) {
								sb.Append( HttpUtility.UrlEncode( option.Value ) );
							}
							else {
								sb.Append( HttpUtility.UrlEncode( option.Option ) );
							}
						}
					}

					return sb.ToString();
				}

				return string.Empty;
			}
		}

		/// <summary>
		/// Unselect all of the control's options.
		/// </summary>
		public void UnselectAll() {
			foreach ( var option in Options ) {
				if ( option.Selected ) {
					option.Selected = false;
				}
			}
		}

		/// <summary>
		/// Gets the control's options.
		/// </summary>
		public ReadOnlyCollection<HtmlOption> Options { get; private set; }
	}
}
