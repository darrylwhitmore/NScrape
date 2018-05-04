using System;
using System.Globalization;
using System.Net;

namespace NScrape {

    internal static class UnexpectedWebResponseGenerator {

        public static Exception CreateException( string message, WebResponse response ) {
            Exception exception;

            switch ( response.ResponseType ) {

				case WebResponseType.Exception:
                    var exceptionResponse = (ExceptionWebResponse)response;

                    exception = exceptionResponse.Exception;
                    break;

				case WebResponseType.Html:
                    exception = new InvalidOperationException( Properties.Resources.UnexpectedHtmlPage );
                    break;

				case WebResponseType.Image:
                    exception = new InvalidOperationException( Properties.Resources.UnexpectedImage );
                    break;

				case WebResponseType.Unsupported:
                    var unsupportedResponse = (UnsupportedWebResponse)response;

                    exception = new WebException( string.Format( CultureInfo.CurrentCulture, Properties.Resources.UnsupportedResponseContentType, unsupportedResponse.ContentType ) );
                    break;

				case WebResponseType.Redirect:
                    exception = new InvalidOperationException( Properties.Resources.UnexpectedRedirect );
                    break;

				case WebResponseType.PlainText:
                    exception = new InvalidOperationException( Properties.Resources.UnexpectedPlainText );
                    break;

				case WebResponseType.Xml:
                    exception = new InvalidOperationException( Properties.Resources.UnexpectedXml );
                    break;

                default:
                    exception = new WebException( string.Format( CultureInfo.CurrentCulture, Properties.Resources.UnsupportedResponseType, response.ResponseType ) );
                    break;
            }

            return new WebException( message, exception );
        }
    }
}
