namespace NScrape {
	public static class WebResponseValidator {

		public static HtmlWebResponse ValidateHtmlResponse( WebResponse response, string message ) {
			return ValidateResponse( response, WebResponseType.Html, message ) as HtmlWebResponse;
        }

        public static ImageWebResponse ValidateImageResponse( WebResponse response, string message ) {
			return ValidateResponse( response, WebResponseType.Image, message ) as ImageWebResponse;
        }

		public static JavaScriptWebResponse ValidateJavaScriptResponse( WebResponse response, string message ) {
			return ValidateResponse( response, WebResponseType.JavaScript, message ) as JavaScriptWebResponse;
		}

		public static PlainTextWebResponse ValidatePlaintextResponse( WebResponse response, string message ) {
			return ValidateResponse( response, WebResponseType.PlainText, message ) as PlainTextWebResponse;
        }

        public static RedirectedWebResponse ValidateRedirectResponse( WebResponse response, string message ) {
			return ValidateResponse( response, WebResponseType.Redirect, message ) as RedirectedWebResponse;
        }

		public static WebResponse ValidateResponse( WebResponse response, WebResponseType validType, string message ) {
			return ValidateResponse( response, new[] { validType }, message );
        }

        public static WebResponse ValidateResponse( WebResponse response, WebResponseType[] validTypes, string message ) {
            var valid = false;

			foreach ( var responseType in validTypes ) {
                if ( response.ResponseType == responseType ) {
                    valid = true;
                    break;
                }
            }

            if ( !valid ) {
                throw UnexpectedWebResponseGenerator.CreateException( message, response );
            }

            return response;
        }

		public static XmlWebResponse ValidateXmlResponse( WebResponse response, string message ) {
			return ValidateResponse( response, WebResponseType.Xml, message ) as XmlWebResponse;
		}
    }
}
