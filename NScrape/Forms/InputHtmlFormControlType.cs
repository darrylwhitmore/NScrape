namespace NScrape.Forms {
	//
	// HTML <input> type Attribute
	// http://www.w3schools.com/tags/att_input_type.asp
	//
	//button 	Defines a clickable button (mostly used with a JavaScript to activate a script)
	//checkbox 	Defines a checkbox
	//file 		Defines an input field and a "Browse..." button, for file uploads
	//hidden 	Defines a hidden input field
	//image 	Defines an image as a submit button
	//password 	Defines a password field. The characters in this field are masked
	//radio 	Defines a radio button
	//reset 	Defines a reset button. A reset button resets all form fields to their initial values
	//submit 	Defines a submit button. A submit button sends form data to a server
	//text 		Defines a one-line input field that a user can enter text into. Default width is 20 characters
	//

	/// <summary>
	/// Indicates the type of HTML control.
	/// </summary>
	public enum InputHtmlFormControlType {
		/// <summary>
		/// Client-side button.
		/// </summary>
		Button,

		/// <summary>
		/// Checkbox.
		/// </summary>
		CheckBox,

		/// <summary>
		/// File selection control.
		/// </summary>
		File,

		/// <summary>
		/// Hidden text field.
		/// </summary>
		Hidden,

		/// <summary>
		/// Image.
		/// </summary>
		Image,

		/// <summary>
		/// Password text field.
		/// </summary>
		Password,

		/// <summary>
		/// Radio button.
		/// </summary>
		Radio,

		/// <summary>
		/// Form reset button.
		/// </summary>
		Reset,

		/// <summary>
		/// Form submission button.
		/// </summary>
		Submit,

		/// <summary>
		/// Text input field.
		/// </summary>
		Text,

		// Dive Into HTML5 - A Form of Madness
		// http://diveintohtml5.org/forms.html
		// W3C Working Draft
		// http://www.w3.org/TR/html-markup/input.html
 		// HTML Living Standard
		// http://www.whatwg.org/specs/web-apps/current-work/multipage/states-of-the-type-attribute.html

		/// <summary>
		/// Color.
		/// </summary>
		Color,

		/// <summary>
		/// Date.
		/// </summary>
		Date,

		/// <summary>
		/// Date/time.
		/// </summary>
		DateTime,

		/// <summary>
		/// Local date/time (no time zone information).
		/// </summary>
		DateTimeLocal,

		/// <summary>
		/// Email address.
		/// </summary>
		Email,

		/// <summary>
		/// Month.
		/// </summary>
		Month,

		/// <summary>
		/// Number.
		/// </summary>
		Number,

		/// <summary>
		/// Range of values.
		/// </summary>
		Range,

		/// <summary>
		/// Search box.
		/// </summary>
		Search,

		/// <summary>
		/// Telephone number.
		/// </summary>
		Tel,

		/// <summary>
		/// Time.
		/// </summary>
		Time,

		/// <summary>
		/// URL.
		/// </summary>
		Url,

		/// <summary>
		/// Week.
		/// </summary>
		Week,

		/// <summary>
		/// Unknown control type.
		/// </summary>
		Unknown
	}
}
