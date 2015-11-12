using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NScrape.Forms {

	/// <summary>
	/// Provides the base implementation for HTML form controls.
	/// </summary>
	public abstract class HtmlFormControl {
	    private readonly Dictionary<string, string> attributes = new Dictionary<string, string>( StringComparer.OrdinalIgnoreCase );

		/// <summary>
		/// Gets the attributes of the control.
		/// </summary>
		public ReadOnlyDictionary<string, string> Attributes { get { return new ReadOnlyDictionary<string, string>( attributes ); } }

        /// <summary>
        /// Parse and add attributes from the control HTML.
        /// </summary>
        /// <param name="html">Contains the control HTML.</param>
        protected void AddAttributes( string html ) {
			foreach ( var attribute in Utility.ParseAttributes( html ) ) {
				attributes.Add( attribute.Key, attribute.Value );
			}

			// http://www.w3.org/TR/html4/interact/forms.html#h-17.12.1
			Disabled = ( Attributes.ContainsKey( "disabled" ) );
        }

        /// <summary>
        /// Adds an individual attribute to the control HTML.
        /// </summary>
        /// <param name="name">
        /// The name of the attribute to add.
        /// </param>
        /// <param name="value">
        /// The attribute value.
        /// </param>
        protected void AddAttribute(string name, string value)
        {
            attributes.Add(name, value);
        }

        /// <summary>
        /// Gets or sets whether or not the control is disabled.
        /// </summary>
        /// <remarks>
        /// Disabled controls are omitted by <see cref="HtmlForm.BuildRequest"/>.
        /// </remarks>
        public bool Disabled { get; set; }

		/// <summary>
		/// Gets the value of the control's <b>id</b> attribute.
		/// </summary>
		/// <remarks>
		/// If the control does not have an <b>id</b> attribute, <b>null</b> is returned.
		/// </remarks>
		public string Id { get { return Attributes.ContainsKey( "id" ) ? Attributes["id"] : null; } }

		/// <summary>
		/// Gets the value of the control's <b>name</b> attribute.
		/// </summary>
		/// <remarks>
		/// If the control does not have an <b>name</b> attribute, <b>null</b> is returned.
		/// </remarks>
		public string Name { get { return Attributes.ContainsKey( "name" ) ? Attributes["name"] : null; } }

		/// <summary>
		/// Gets the value of the control in <b>application/x-www-form-urlencoded</b> format.
		/// </summary>
		public abstract string EncodedData { get; }
    }
}
