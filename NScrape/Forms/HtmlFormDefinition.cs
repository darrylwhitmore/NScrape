using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace NScrape.Forms {

	/// <summary>
	/// Represents the definition of an HTML form.
	/// </summary>
	public class HtmlFormDefinition {
		private readonly Dictionary<string, string> attributes = new Dictionary<string, string>( StringComparer.OrdinalIgnoreCase );
		private readonly List<HtmlFormControl> controls = new List<HtmlFormControl>();

		private HtmlFormDefinition() {
		}

		/// <summary>
		/// Gets the attributes of the HTML form.
		/// </summary>
		public ReadOnlyDictionary<string, string> Attributes { get { return new ReadOnlyDictionary<string, string>( attributes ); } }

		/// <summary>
		/// Gets the controls contained within the HTML form.
		/// </summary>
		public IEnumerable<HtmlFormControl> Controls { get { return controls; } }

		/// <summary>
		/// Gets the <b>id</b> attribute of the HTML form.
		/// </summary>
		/// <remarks>
		/// If the HTML form does not have an <b>id</b> attribute, <b>null</b> is returned.
		/// </remarks>
		public string Id { get { return Attributes.ContainsKey( "id" ) ? Attributes["id"] : null; } }

		/// <summary>
		/// Gets the HTML text of the page containing the form.
		/// </summary>
		public string PageHtml { get; private set; }

		/// <summary>
		/// Parses HTML form definitions out of  HTML text.
		/// </summary>
		/// <param name="html">Contains the HTML text to be parsed.</param>
		/// <returns>A collection of <see cref="HtmlFormDefinition"/> objects</returns>
		public static IEnumerable<HtmlFormDefinition> Parse( string html ) {
			var formMatches = RegexCache.Instance.Regex( RegexLibrary.ParseForms, RegexLibrary.ParseFormsOptions ).Matches( html );

			return ( from Match formMatch in formMatches select PopulateForm( formMatch, html ) ).ToList();
		}

		private static HtmlFormDefinition PopulateForm( Match formMatch, string pageHtml ) {
			var parsedForm = new HtmlFormDefinition {
				PageHtml = pageHtml
			};

			foreach ( var attribute in Utility.ParseAttributes( formMatch.Groups[RegexLibrary.ParseFormsAttributesGroup].Value ) ) {
				parsedForm.attributes.Add( attribute.Key, attribute.Value );
			}

			// TODO: need to remove comments from form HTML before parsing out controls; commented out controls will be found!

			// Populate controls
			var controlMatches = RegexCache.Instance.Regex( RegexLibrary.ParseFormControls, RegexLibrary.ParseFormControlsOptions ).Matches( formMatch.Groups[RegexLibrary.ParseFormsBodyGroup].Value );

			foreach ( Match controlMatch in controlMatches ) {
				HtmlFormControl control;

				if ( controlMatch.Groups[RegexLibrary.ParseFormControlsInputGroup].Value.Length > 0 ) {
					var inputControl = new InputHtmlFormControl( controlMatch.Value );

					if ( inputControl.ControlType == InputHtmlFormControlType.Radio ) {
						control = new InputRadioHtmlFormControl( controlMatch.Value );
					}
					else if ( inputControl.ControlType == InputHtmlFormControlType.CheckBox ) {
						control = new InputCheckBoxHtmlFormControl( controlMatch.Value );
					}
					else {
						// Generic control
						control = inputControl;
					}
				}
				else if ( controlMatch.Groups[RegexLibrary.ParseFormControlsSelectGroup].Value.Length > 0 ) {
					control = new SelectHtmlFormControl( controlMatch.Value );
				}
				else if ( controlMatch.Groups[RegexLibrary.ParseFormControlsTextAreaGroup].Value.Length > 0 ) {
					control = new TextAreaHtmlFormControl( controlMatch.Value );
				}
				else {
					throw new System.Net.WebException( string.Format( CultureInfo.CurrentCulture, Properties.Resources.UnsupportedHtmlControl, controlMatch.Value ) );
				}

				if ( control.Name != null ) {
					parsedForm.controls.Add( control );
				}
			}

			return parsedForm;
		}
	}
}
