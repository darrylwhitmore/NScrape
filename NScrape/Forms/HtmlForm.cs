using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace NScrape.Forms {

	/// <summary>
	/// Provides the base implementation for HTML form functionality.
	/// </summary>
	public abstract class HtmlForm {
		private const string EventTargetName = "__EVENTTARGET";
		private const string EventArgumentName = "__EVENTARGUMENT";

		/// <summary>
		/// Initializes a new instance of the <see cref="HtmlForm"/> class.
		/// </summary>
		/// <param name="webClient">Contains the web client to be used to request and submit the form.</param>
		protected HtmlForm( IWebClient webClient ) {
			WebClient = webClient;

			Attributes = new Dictionary<string, string>( StringComparer.OrdinalIgnoreCase );
			Controls = new List<HtmlFormControl>();
		}

		/// <summary>
		/// Loads the form specified by ordinal on the page at the specified URL.
		/// </summary>
		/// <param name="formUrl">Contains the URL of the page containing the form.</param>
		/// <param name="formOrdinal">Contains the zero-based ordinal of the form to load.</param>
		/// <remarks>
		/// The specified (zero-based ordinal) form in the page shall be loaded.
		/// </remarks>
		public void Load( Uri formUrl, int formOrdinal = 0 ) {
			FormUrl = formUrl;

			Html = DownloadFormHtml();

			Initialize( formOrdinal );
		}

		/// <summary>
		/// Loads the form specified by identifying attributes on the page at the specified URL.
		/// </summary>
		/// <param name="formUrl">Contains the URL of the page containing the form.</param>
		/// <param name="attribute">Contains the form attribute to be used to identify the form to load.</param>
		/// <param name="attributeValue">Contains the form attribute value to be used to identify the form to load.</param>
		/// <remarks>
		/// The form identified by the specified attribute/value shall be loaded.
		/// </remarks>
		public void Load( Uri formUrl, string attribute, string attributeValue ) {
			FormUrl = formUrl;

			Html = DownloadFormHtml();

			Initialize( attribute, attributeValue );
		}

		/// <summary>
		/// Loads the form specified by ordinal in the provided HTML.
		/// </summary>
		/// <param name="formUrl">Contains the URL where the page containing the form resides.</param>
		/// <param name="formHtml">Contains the HTML text containing the form.</param>
		/// <param name="formOrdinal">Contains the zero-based ordinal of the form to load.</param>
		/// <remarks>
		/// The specified (zero-based ordinal) form in the provided HTML text shall be loaded.
		/// </remarks>
		public void Load( Uri formUrl, string formHtml, int formOrdinal = 0 ) {
			FormUrl = formUrl;

			Html = formHtml;

			Initialize( formOrdinal );
		}

		/// <summary>
		/// Loads the form specified by identifying attributes in the provided HTML.
		/// </summary>
		/// <param name="formUrl">Contains the URL where the page containing the form resides.</param>
		/// <param name="formHtml">Contains the HTML text containing the form.</param>
		/// <param name="attribute">Contains the form attribute to be used to identify the form to load.</param>
		/// <param name="attributeValue">Contains the form attribute value to be used to identify the form to load.</param>
		/// <remarks>
		/// The form identified by the specified attribute/value in the provided HTML text shall be loaded.
		/// </remarks>
		public void Load( Uri formUrl, string formHtml, string attribute, string attributeValue ) {
			FormUrl = formUrl;

			Html = formHtml;

			Initialize( attribute, attributeValue );
		}

		/// <summary>
		/// Loads the form specified by the provided form definition.
		/// </summary>
		/// <param name="formUrl">Contains the URL where the page containing the form resides.</param>
		/// <param name="formDefinition">Contains the form definition.</param>
		public void Load( Uri formUrl, HtmlFormDefinition formDefinition ) {
			FormUrl = formUrl;

			Html = formDefinition.PageHtml;

			PopulateForm( formDefinition );
		}

		protected Uri ActionUrl {
			get {
				if ( Attributes.ContainsKey( "action" ) ) {
					if ( Uri.IsWellFormedUriString( Attributes["action"], UriKind.Absolute ) ) {
						return new Uri( Attributes["action"] );
					}

					return new Uri( FormUrl, new Uri( Attributes["action"], UriKind.Relative ) );
				}

				// If the ACTION is missing, the URL for the document itself is assumed
				// http://www.w3.org/MarkUp/html3/forms.html
				return FormUrl;
			}
		}

		/// <summary>
		/// Gets the attributes of the form.
		/// </summary>
		protected Dictionary<string, string> Attributes { get; private set; }

		/// <summary>
		/// Gets the controls contained within the HTML form.
		/// </summary>
		protected List<HtmlFormControl> Controls { get; private set; }

		/// <summary>
		/// Gets the URL of the page where the form is located.
		/// </summary>
		public Uri FormUrl { get; protected set; }

		/// <summary>
		/// Gets the HTML text of the page containing the form.
		/// </summary>
		protected string Html { get; private set; }

		/// <summary>
		/// Gets the web client used to request and submit the form.
		/// </summary>
		protected IWebClient WebClient { get; private set; }

		/// <summary>
		/// Builds the request data to be used to submit an ASPX form.
		/// </summary>
		/// <param name="eventTargetValue">Contains the value for <b>__EVENTTARGET</b>, the control doing the submission.</param>
		/// <param name="eventArgumentValue">Contains the value for <b>__EVENTARGUMENT</b>, any additional information.</param>
		/// <returns>The request data in <b>application/x-www-form-urlencoded</b> format.</returns>
		/// <exception cref="InvalidOperationException">The form does not contain an <b>__EVENTTARGET</b> or <b>__EVENTARGUMENT</b> control.</exception>
		/// <remarks>
		/// See <see href="http://www.evagoras.com/2011/02/10/how-postback-works-in-asp-net/">How postback works in ASP.NET</see> for a good overview on the topic.
		/// </remarks>
		protected string BuildAspxPostBackRequest( string eventTargetValue, string eventArgumentValue ) {
			var eventTarget = Controls.SingleOrDefault( c => c.Name == EventTargetName ) as InputHtmlFormControl;
			if ( eventTarget == null ) {
				throw new InvalidOperationException( NScrapeResources.CanNotPostBack );
			}

			var eventArgument = Controls.SingleOrDefault( c => c.Name == EventArgumentName ) as InputHtmlFormControl;
			if ( eventArgument == null ) {
				throw new InvalidOperationException( NScrapeResources.CanNotPostBack );
			}

			// Most __doPostBack() examples found via Googling unescaped the event target parameter,
			// but some did not, so play it safe and see which kind we have
			if ( RegexCache.Instance.Regex( RegexLibrary.MatchDoPostBack, RegexLibrary.MatchDoPostBackOptions ).IsMatch( Html ) ) {
				eventTarget.Value = eventTargetValue.Replace( '$', ':' );
			}
			else {
				eventTarget.Value = eventTargetValue;
			}

			eventArgument.Value = eventArgumentValue;

			// Build the request using an empty string so that none of the buttons in the form are included.
			var request = BuildRequest( string.Empty );

			// Do not persist the values
			eventTarget.Value = string.Empty;
			eventArgument.Value = string.Empty;

			return request;
		}

		/// <summary>
		/// Builds the request data to be used to submit an HTML form.
		/// </summary>
		/// <param name="submitButtonName">Contains the <b>name</b> attribute of the submit button or image.</param>
		/// <returns>The request data in <b>application/x-www-form-urlencoded</b> format.</returns>
		/// <remarks>
		/// When the form has multiple submit buttons/images, specify which one should be used; otherwise, for forms with a 
		/// single submit button/image, the parameter may be omitted.
		/// <br/><br/>
		/// The values of any <see cref="HtmlFormControl.Disabled">disabled</see> controls are omitted.
		/// </remarks>
		protected string BuildRequest( string submitButtonName = null ) {
			var builder = new StringBuilder();

			foreach ( var control in Controls ) {
				var includeControl = true;

				if ( control.Disabled ) {
					// The control is disabled and shall not be submitted
					includeControl = false;
				}
				else {
					var inputControl = control as InputHtmlFormControl;

					if ( inputControl != null ) {
						switch ( inputControl.ControlType ) {
							case InputHtmlFormControlType.Button:
								// The control is a client-side button
								includeControl = false;
								break;

							case InputHtmlFormControlType.Radio:
								var radioControl = inputControl as InputRadioHtmlFormControl;

								if ( radioControl != null && !radioControl.Checked ) {
									// The control is a radio button that is not checked
									includeControl = false;
								}
								break;

							case InputHtmlFormControlType.CheckBox:
								var checkControl = inputControl as InputCheckBoxHtmlFormControl;

								if ( checkControl != null && !checkControl.Checked ) {
									// The control is a checkbox that is not checked
									includeControl = false;
								}
								break;

							case InputHtmlFormControlType.Image:
							case InputHtmlFormControlType.Submit:
								if ( submitButtonName != null && inputControl.Name.ToUpperInvariant() != submitButtonName.ToUpperInvariant() ) {
									// The form apparently has multiple submit buttons, but this one is not the one that we want
									includeControl = false;
								}
								break;
						}
					}
				}

				if ( includeControl && control.Name.Length > 0 ) {
					if ( builder.Length > 0 ) {
						builder.Append( "&" );
					}

					builder.Append( control.EncodedData );
				}
			}

			return builder.ToString();
		}

		private string DownloadFormHtml() {
			var response = WebClient.SendRequest( new GetWebRequest( FormUrl ) );

			var htmlResponse = WebResponseValidator.ValidateHtmlResponse( response, string.Format( CultureInfo.CurrentCulture, NScrapeResources.UnexpectedResponseOnFormPageRequest, FormUrl ) );

			FormUrl = htmlResponse.ResponseUrl;

			return htmlResponse.Html;
		}

		private void Initialize( int formOrdinal ) {
			var formDefinitions = HtmlFormDefinition.Parse( Html ).ToList();

			if ( formOrdinal < 0 || formOrdinal >= formDefinitions.Count() ) {
				throw new ArgumentException( string.Format( CultureInfo.CurrentCulture, NScrapeResources.InvalidFormOrdinal, formOrdinal ) );
			}

			PopulateForm( formDefinitions.ElementAt( formOrdinal ) );
		}

		private void Initialize( string attribute, string attributeValue ) {
			var formDefinitions = HtmlFormDefinition.Parse( Html ).ToList();

			var formDefinition = formDefinitions.FirstOrDefault( d => d.Attributes.ContainsKey( attribute ) && d.Attributes[attribute] == attributeValue );

			if ( formDefinition == null ) {
				throw new ArgumentException( string.Format( CultureInfo.CurrentCulture, NScrapeResources.InvalidFormId, attribute.ToUpperInvariant(), attributeValue ) );
			}

			PopulateForm( formDefinition );
		}

		private void PopulateForm( HtmlFormDefinition formDefinition ) {

			foreach ( var attribute in formDefinition.Attributes ) {
				Attributes.Add( attribute.Key, attribute.Value );
			}

			foreach ( var control in formDefinition.Controls ) {
				Controls.Add( control );
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the form submission shall attempt to mimic a JQuery request.
		/// </summary>
		/// <remarks>
		/// If <b>true</b>, the form request shall have the <b>X-Requested-With=XMLHttpRequest</b> header added to the headers collection. If <b>false</b>,
		/// the header shall be removed if previously added.
		/// </remarks>
		protected bool SubmitAsXmlHttpRequest { get; set; }

		/// <summary>
		/// Submits the form, specifying the request data.
		/// </summary>
		/// <param name="requestData">Contains the request data in <b>application/x-www-form-urlencoded</b> format.</param>
		/// <returns>The HTML response from the server.</returns>
		/// <remarks>
		/// This method is convenient for forms that are known to return an HTML response. Requests shall be automatically redirected if necessary.
		/// </remarks>
		protected HtmlWebResponse SubmitHtmlRequest( string requestData ) {
			return SubmitRequest( requestData, WebResponseType.Html ) as HtmlWebResponse;
		}

		/// <summary>
		/// Submits the form, specifying the request data.
		/// </summary>
		/// <param name="requestData">Contains the request data in <b>application/x-www-form-urlencoded</b> format.</param>
		/// <returns>The redirection response from the server.</returns>
		/// <remarks>
		/// This method is convenient for forms that are known to return a redirection response. 
		/// </remarks>
		protected RedirectedWebResponse SubmitRedirectRequest( string requestData ) {
			return SubmitRequest( requestData, false, new[] { WebResponseType.Redirect } ) as RedirectedWebResponse;
		}

		/// <summary>
		/// Submits the form, specifying the request data and the expected response type.
		/// </summary>
		/// <param name="requestData">Contains the request data in <b>application/x-www-form-urlencoded</b> format.</param>
		/// <param name="validType">Contains the expected response type.</param>
		/// <returns>The response from the server.</returns>
		/// <remarks>
		/// This overload is convenient for forms that are known to return a specific response type. Requests shall be automatically redirected if necessary.
		/// </remarks>
		protected WebResponse SubmitRequest( string requestData, WebResponseType validType ) {
			return SubmitRequest( requestData, true, new[] { validType } );
		}

		/// <summary>
		/// Submits the form, specifying the request data, redirection action, and the expected response type.
		/// </summary>
		/// <param name="requestData">Contains the request data in <b>application/x-www-form-urlencoded</b> format.</param>
		/// <param name="autoRedirect"><b>true</b> if the request should be automatically redirected; <b>false</b> otherwise.</param>
		/// <param name="validType">Contains the expected response type.</param>
		/// <returns>The response from the server.</returns>
		/// <remarks>
		/// This overload is convenient when the redirection action needs to be specified.
		/// </remarks>
		protected WebResponse SubmitRequest( string requestData, bool autoRedirect, WebResponseType validType ) {
			return SubmitRequest( requestData, autoRedirect, new[] { validType } );
		}

		/// <summary>
		/// Submits the form, specifying the request data and a range of valid response types.
		/// </summary>
		/// <param name="requestData">Contains the request data in <b>application/x-www-form-urlencoded</b> format.</param>
		/// <param name="validTypes">Contains a list of acceptable response types.</param>
		/// <returns>The response from the server.</returns>
		/// <remarks>
		/// This overload is convenient when multiple response types are possible. Requests shall be automatically redirected if necessary.
		/// </remarks>
		protected WebResponse SubmitRequest( string requestData, WebResponseType[] validTypes ) {
			return SubmitRequest( requestData, true, validTypes );
		}

		/// <summary>
		/// Submits the form, specifying the request data, redirection action, and a range of valid response types.
		/// </summary>
		/// <param name="requestData">Contains the request data in <b>application/x-www-form-urlencoded</b> format.</param>
		/// <param name="autoRedirect"><b>true</b> if the request should be automatically redirected; <b>false</b> otherwise.</param>
		/// <param name="validTypes">Contains a list of acceptable response types.</param>
		/// <returns>The response from the server.</returns>
		/// <remarks>
		/// This overload allows all form request options to be specified.
		/// </remarks>
		protected WebResponse SubmitRequest( string requestData, bool autoRedirect, WebResponseType[] validTypes ) {
			WebRequest request;

			if ( Attributes.ContainsKey( "method" ) && Attributes["method"].ToUpperInvariant() == "POST" ) {
				request = new PostWebRequest( ActionUrl, requestData, autoRedirect );
			}
			else {
				var builder = new UriBuilder( ActionUrl ) {
					Query = requestData,
					Port = -1
				};

				request = new GetWebRequest( builder.Uri, autoRedirect );
			}

			request.IsXmlHttpRequest = SubmitAsXmlHttpRequest;

            if (FormUrl != null) {
                request.Headers.Add(CommonHeaders.Referer, FormUrl.ToString());
            }

			return WebResponseValidator.ValidateResponse( WebClient.SendRequest( request ), validTypes, string.Format( CultureInfo.CurrentCulture, NScrapeResources.UnexpectedResponseOnFormSubmission, request.Destination ) );
		}
    }
}
