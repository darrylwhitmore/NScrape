using System;
using System.Linq;
using NScrape.Interfaces;
using NScrape.Responses;

namespace NScrape {
	/// <summary>
	/// Provides functionality to validate web responses.
	/// </summary>
	public static class WebResponseValidator {

		/// <summary>
		/// Validates a <see cref="IWebResponse"/> to be of type <see cref="WebResponseType.Html"/>.
		/// </summary>
		/// <param name="response">The web response to be validated.</param>
		/// <param name="message">The error message to be used to generate an exception if the validation fails.</param>
		/// <returns>The web response cast as an <see cref="IHtmlWebResponse"/>.</returns>
		public static IHtmlWebResponse ValidateHtmlResponse( IWebResponse response, string message ) {
			return ValidateResponse( response, WebResponseType.Html, message ) as IHtmlWebResponse;
        }

        /// <summary>
		/// Validates a <see cref="IWebResponse"/> to be of type <see cref="WebResponseType.Image"/>.
        /// </summary>
		/// <param name="response">The web response to be validated.</param>
		/// <param name="message">The error message to be used to generate an exception if the validation fails.</param>
		/// <returns>The web response cast as an <see cref="IImageWebResponse"/>.</returns>
		public static IImageWebResponse ValidateImageResponse( IWebResponse response, string message ) {
			return ValidateResponse( response, WebResponseType.Image, message ) as IImageWebResponse;
        }

		/// <summary>
		/// Validates a <see cref="IWebResponse"/> to be of type <see cref="WebResponseType.JavaScript"/>.
		/// </summary>
		/// <param name="response">The web response to be validated.</param>
		/// <param name="message">The error message to be used to generate an exception if the validation fails.</param>
		/// <returns>The web response cast as a <see cref="IJavaScriptWebResponse"/>.</returns>
		public static IJavaScriptWebResponse ValidateJavaScriptResponse( IWebResponse response, string message ) {
			return ValidateResponse( response, WebResponseType.JavaScript, message ) as IJavaScriptWebResponse;
		}

		/// <summary>
		/// Validates a <see cref="IWebResponse"/> to be of type <see cref="WebResponseType.PlainText"/>.
		/// </summary>
		/// <param name="response">The web response to be validated.</param>
		/// <param name="message">The error message to be used to generate an exception if the validation fails.</param>
		/// <returns>The web response cast as a <see cref="IPlainTextWebResponse"/>.</returns>
		public static IPlainTextWebResponse ValidatePlaintextResponse( IWebResponse response, string message ) {
			return ValidateResponse( response, WebResponseType.PlainText, message ) as IPlainTextWebResponse;
        }

		/// <summary>
		/// Validates a <see cref="IWebResponse"/> to be of type <see cref="WebResponseType.Redirect"/>.
		/// </summary>
		/// <param name="response">The web response to be validated.</param>
		/// <param name="message">The error message to be used to generate an exception if the validation fails.</param>
		/// <returns>The web response cast as a <see cref="IRedirectedWebResponse"/>.</returns>
		public static IRedirectedWebResponse ValidateRedirectResponse( IWebResponse response, string message ) {
			return ValidateResponse( response, WebResponseType.Redirect, message ) as IRedirectedWebResponse;
        }

		/// <summary>
		/// Validates a <see cref="IWebResponse"/> to be of a given type.
		/// </summary>
		/// <param name="response">The web response to be validated.</param>
		/// <param name="validType">The valid web response type.</param>
		/// <param name="message">The error message to be used to generate an exception if the validation fails.</param>
		/// <returns>The web response.</returns>
		public static IWebResponse ValidateResponse( IWebResponse response, WebResponseType validType, string message ) {
			return ValidateResponse( response, new[] { validType }, message );
        }

        /// <summary>
		/// Validates a <see cref="IWebResponse"/> to be of a given range of types.
        /// </summary>
		/// <param name="response">The web response to be validated.</param>
        /// <param name="validTypes">The range of valid web response types.</param>
		/// <param name="message">The error message to be used to generate an exception if the validation fails.</param>
		/// <returns>The web response.</returns>
        /// <exception cref="Exception">The web response is not valid.</exception>
        public static IWebResponse ValidateResponse( IWebResponse response, WebResponseType[] validTypes, string message ) {
            var valid = validTypes.Any( t => response.ResponseType == t );

	        if ( !valid ) {
                throw UnexpectedWebResponseGenerator.CreateException( message, response );
            }

            return response;
        }

		/// <summary>
		/// Validates a <see cref="IWebResponse"/> to be of type <see cref="WebResponseType.Xml"/>.
		/// </summary>
		/// <param name="response">The web response to be validated.</param>
		/// <param name="message">The error message to be used to generate an exception if the validation fails.</param>
		/// <returns>The web response cast as an <see cref="IXmlWebResponse"/>.</returns>
		public static IXmlWebResponse ValidateXmlResponse( IWebResponse response, string message ) {
			return ValidateResponse( response, WebResponseType.Xml, message ) as IXmlWebResponse;
		}
    }
}
