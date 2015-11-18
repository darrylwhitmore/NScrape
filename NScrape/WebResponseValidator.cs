using System;
using System.Linq;

namespace NScrape {
	/// <summary>
	/// Provides functionality to validate web responses.
	/// </summary>
	public static class WebResponseValidator {

		/// <summary>
		/// Validates a <see cref="WebResponse"/> to be of type <see cref="WebResponseType.Html"/>.
		/// </summary>
		/// <param name="response">The web response to be validated.</param>
		/// <param name="message">The error message to be used to generate an exception if the validation fails.</param>
		/// <returns>The web response cast as an <see cref="HtmlWebResponse"/>.</returns>
		public static HtmlWebResponse ValidateHtmlResponse( WebResponse response, string message ) {
			return ValidateResponse( response, WebResponseType.Html, message ) as HtmlWebResponse;
        }

        /// <summary>
		/// Validates a <see cref="WebResponse"/> to be of type <see cref="WebResponseType.Image"/>.
        /// </summary>
		/// <param name="response">The web response to be validated.</param>
		/// <param name="message">The error message to be used to generate an exception if the validation fails.</param>
		/// <returns>The web response cast as an <see cref="ImageWebResponse"/>.</returns>
		public static ImageWebResponse ValidateImageResponse( WebResponse response, string message ) {
			return ValidateResponse( response, WebResponseType.Image, message ) as ImageWebResponse;
        }

		/// <summary>
		/// Validates a <see cref="WebResponse"/> to be of type <see cref="WebResponseType.JavaScript"/>.
		/// </summary>
		/// <param name="response">The web response to be validated.</param>
		/// <param name="message">The error message to be used to generate an exception if the validation fails.</param>
		/// <returns>The web response cast as a <see cref="JavaScriptWebResponse"/>.</returns>
		public static JavaScriptWebResponse ValidateJavaScriptResponse( WebResponse response, string message ) {
			return ValidateResponse( response, WebResponseType.JavaScript, message ) as JavaScriptWebResponse;
		}

		/// <summary>
		/// Validates a <see cref="WebResponse"/> to be of type <see cref="WebResponseType.PlainText"/>.
		/// </summary>
		/// <param name="response">The web response to be validated.</param>
		/// <param name="message">The error message to be used to generate an exception if the validation fails.</param>
		/// <returns>The web response cast as a <see cref="PlainTextWebResponse"/>.</returns>
		public static PlainTextWebResponse ValidatePlaintextResponse( WebResponse response, string message ) {
			return ValidateResponse( response, WebResponseType.PlainText, message ) as PlainTextWebResponse;
        }

		/// <summary>
		/// Validates a <see cref="WebResponse"/> to be of type <see cref="WebResponseType.Redirect"/>.
		/// </summary>
		/// <param name="response">The web response to be validated.</param>
		/// <param name="message">The error message to be used to generate an exception if the validation fails.</param>
		/// <returns>The web response cast as a <see cref="RedirectedWebResponse"/>.</returns>
		public static RedirectedWebResponse ValidateRedirectResponse( WebResponse response, string message ) {
			return ValidateResponse( response, WebResponseType.Redirect, message ) as RedirectedWebResponse;
        }

		/// <summary>
		/// Validates a <see cref="WebResponse"/> to be of a given type.
		/// </summary>
		/// <param name="response">The web response to be validated.</param>
		/// <param name="validType">The valid web response type.</param>
		/// <param name="message">The error message to be used to generate an exception if the validation fails.</param>
		/// <returns>The web response.</returns>
		public static WebResponse ValidateResponse( WebResponse response, WebResponseType validType, string message ) {
			return ValidateResponse( response, new[] { validType }, message );
        }

        /// <summary>
		/// Validates a <see cref="WebResponse"/> to be of a given range of types.
        /// </summary>
		/// <param name="response">The web response to be validated.</param>
        /// <param name="validTypes">The range of valid web response types.</param>
		/// <param name="message">The error message to be used to generate an exception if the validation fails.</param>
		/// <returns>The web response.</returns>
        /// <exception cref="Exception">The web response is not valid.</exception>
        public static WebResponse ValidateResponse( WebResponse response, WebResponseType[] validTypes, string message ) {
            var valid = validTypes.Any( t => response.ResponseType == t );

	        if ( !valid ) {
                throw UnexpectedWebResponseGenerator.CreateException( message, response );
            }

            return response;
        }

		/// <summary>
		/// Validates a <see cref="WebResponse"/> to be of type <see cref="WebResponseType.Xml"/>.
		/// </summary>
		/// <param name="response">The web response to be validated.</param>
		/// <param name="message">The error message to be used to generate an exception if the validation fails.</param>
		/// <returns>The web response cast as an <see cref="XmlWebResponse"/>.</returns>
		public static XmlWebResponse ValidateXmlResponse( WebResponse response, string message ) {
			return ValidateResponse( response, WebResponseType.Xml, message ) as XmlWebResponse;
		}
    }
}
