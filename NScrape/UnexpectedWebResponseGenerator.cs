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
                    exception = new InvalidOperationException( NScrapeResources.UnexpectedHtmlPage );
                    break;

				case WebResponseType.Image:
                    exception = new InvalidOperationException( NScrapeResources.UnexpectedImage );
                    break;

				case WebResponseType.Unsupported:
                    var unsupportedResponse = (UnsupportedWebResponse)response;

                    exception = new WebException( string.Format( CultureInfo.CurrentCulture, NScrapeResources.UnsupportedResponseContentType, unsupportedResponse.ContentType ) );
                    break;

				case WebResponseType.Redirect:
                    exception = new InvalidOperationException( NScrapeResources.UnexpectedRedirect );
                    break;

				case WebResponseType.PlainText:
                    exception = new InvalidOperationException( NScrapeResources.UnexpectedPlainText );
                    break;

				case WebResponseType.Xml:
                    exception = new InvalidOperationException( NScrapeResources.UnexpectedXml );
                    break;

                default:
                    exception = new WebException( string.Format( CultureInfo.CurrentCulture, NScrapeResources.UnsupportedResponseType, response.ResponseType ) );
                    break;
            }

            return new WebException( message, exception );
        }
    }
}
