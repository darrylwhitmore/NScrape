using System.Collections.Generic;
using System.Linq;

namespace NScrape.Forms {
	/// <summary>
	/// Provides the base implementation for basic HTML forms.
	/// </summary>
	/// <remarks>
	/// This class is intended to provide a basic foundation for building form classes. It exposes control collections by type for
	/// easy usage.
	/// <br/><br/>
	/// If you need additional or specialized functionality, you can create a form class that 
	/// inherits from <see cref="HtmlForm"/>.
	/// </remarks>
	public abstract class BasicForm : HtmlForm {
		private List<HtmlFormControl> allControls;
		private List<InputCheckBoxHtmlFormControl> checkBoxControls;
		private List<InputHtmlFormControl> inputControls;
		private List<InputRadioHtmlFormControl> radioControls;
		private List<SelectHtmlFormControl> selectControls;
		private List<TextAreaHtmlFormControl> textAreaControls;

		/// <summary>
		/// Initializes a new instance of the <see cref="BasicForm"/> class.
		/// </summary>
		/// <param name="webClient">Contains the web client to be used to request and submit the form.</param>
		protected BasicForm( IWebClient webClient )
			: base( webClient ) {
		}

		/// <summary>
		/// Gets all HTML controls contained within the form.
		/// </summary>
		public IEnumerable<HtmlFormControl> AllControls {
			get {
				if ( allControls == null ) {
					allControls = Controls.ToList();
				}

				return allControls;
			}
		}

		/// <summary>
		/// Gets the HTML <b>input checkbox</b> controls contained within the form.
		/// </summary>
		public IEnumerable<InputCheckBoxHtmlFormControl> CheckBoxControls {
			get {
				if ( checkBoxControls == null ) {
					checkBoxControls = Controls.Where( c => c is InputCheckBoxHtmlFormControl ).Cast<InputCheckBoxHtmlFormControl>().ToList();
				}

				return checkBoxControls;
			}
		}

		/// <summary>
		/// Gets the  HTML <b>input</b> controls contained within the form.
		/// </summary>
		public IEnumerable<InputHtmlFormControl> InputControls {
			get {
				if ( inputControls == null ) {
					inputControls = Controls.Where( c => c.GetType().IsAssignableFrom( typeof ( InputHtmlFormControl ) ) ).Cast<InputHtmlFormControl>().ToList();
				}

				return inputControls; 
			}
		}

		/// <summary>
		/// Gets the  HTML <b>input radio</b> controls contained within the form.
		/// </summary>
		public IEnumerable<InputRadioHtmlFormControl> RadioControls {
			get {
				if ( radioControls == null ) {
					radioControls = Controls.Where( c => c is InputRadioHtmlFormControl ).Cast<InputRadioHtmlFormControl>().ToList();
				}

				return radioControls;
			}
		}

		/// <summary>
		/// Gets the  HTML <b>select</b> controls contained within the form.
		/// </summary>
		public IEnumerable<SelectHtmlFormControl> SelectControls {
			get {
				if ( selectControls == null ) {
					selectControls = Controls.Where( c => c is SelectHtmlFormControl ).Cast<SelectHtmlFormControl>().ToList();
				}

				return selectControls;
			}
		}

		/// <summary>
		/// Gets the  HTML <b>textarea</b> controls contained within the form.
		/// </summary>
		public IEnumerable<TextAreaHtmlFormControl> TextAreaControls {
			get {
				if ( textAreaControls == null ) {
					textAreaControls = Controls.Where( c => c is TextAreaHtmlFormControl ).Cast<TextAreaHtmlFormControl>().ToList();
				}

				return textAreaControls;
			}
		}
	}
}
