using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using NScrape.Responses;
using WebResponse = NScrape.Responses.WebResponse;

namespace NScrape {
	/// <summary>
	/// Creates a <see cref="WebResponse"/> object based on an <see cref="HttpWebResponse"/> object.
	/// </summary>
	public class WebResponseFactory {
		/// <summary>
		/// Initializes static members of the <see cref="WebResponseFactory"/> class.
		/// </summary>
		static WebResponseFactory() {
			SupportedContentTypes = new Dictionary<string, Func<HttpWebResponse, WebResponse>> {
				{ "application/javascript", CreateJavaScriptResponse },
				{ "application/json", CreateJsonResponse },
				{ "application/octet-stream", CreateBinaryResponse },
				{ "application/x-dosexec", CreateBinaryResponse },
				{ "application/x-javascript", CreateJavaScriptResponse },
				{ "application/x-msdos-program", CreateBinaryResponse },
				{ "application/xml", CreateXmlResponse },
				{ "image/", CreateImageResponse },
				{ "text/html", CreateHtmlResponse },
				{ "text/javascript", CreateJavaScriptResponse },
				{ "text/plain", CreatePlainTextResponse },
				{ "text/xml", CreateXmlResponse }
			};
		}

		/// <summary>
		/// Gets a dictionary containing all content types currently supported by the
		/// <see cref="WebResponseFactory"/>.
		/// </summary>
		/// <remarks>
		/// The key of the dictionary is the text the content type
		/// must start with (but not necessarily the full text of the content type).
		/// <br/><br/>
		/// The value is a
		/// <see cref="Func{HttpWebResponse, WebResponse}"/> that takes an <see cref="HttpWebResponse"/>
		/// object with the given content type and returns a <see cref="WebResponse"/>. The return value
		/// is usually a subclass of the <see cref="WebResponse"/> class.
		/// </remarks>
		public static Dictionary<string, Func<HttpWebResponse, WebResponse>> SupportedContentTypes { get; }

		/// <summary>
		/// Creates an <see cref="HtmlWebResponse"/>.
		/// </summary>
		/// <param name="webResponse">
		/// The original <see cref="HttpWebResponse"/>.
		/// </param>
		/// <returns>
		/// A new <see cref="HtmlWebResponse"/>.
		/// </returns>
		public static WebResponse CreateHtmlResponse( HttpWebResponse webResponse ) {
			return new HtmlWebResponse( true, webResponse );
		}

		/// <summary>
		/// Creates a <see cref="ImageWebResponse"/>.
		/// </summary>
		/// <param name="webResponse">
		/// The original <see cref="HttpWebResponse"/>.
		/// </param>
		/// <returns>
		/// A new <see cref="ImageWebResponse"/>.
		/// </returns>
		public static WebResponse CreateImageResponse( HttpWebResponse webResponse ) {
			return new ImageWebResponse( true, webResponse );
		}

		/// <summary>
		/// Creates a <see cref="PlainTextWebResponse"/>.
		/// </summary>
		/// <param name="webResponse">
		/// The original <see cref="HttpWebResponse"/>.
		/// </param>
		/// <returns>
		/// A new <see cref="PlainTextWebResponse"/>.
		/// </returns>
		public static WebResponse CreatePlainTextResponse( HttpWebResponse webResponse ) {
			return new PlainTextWebResponse( true, webResponse );
		}

		/// <summary>
		/// Creates an <see cref="XmlWebResponse"/>.
		/// </summary>
		/// <param name="webResponse">
		/// The original <see cref="HttpWebResponse"/>.
		/// </param>
		/// <returns>
		/// A new <see cref="XmlWebResponse"/>.
		/// </returns>
		public static WebResponse CreateXmlResponse( HttpWebResponse webResponse ) {
			return new XmlWebResponse( true, webResponse );
		}

		/// <summary>
		/// Creates a <see cref="BinaryWebResponse"/>.
		/// </summary>
		/// <param name="webResponse">
		/// The original <see cref="HttpWebResponse"/>.
		/// </param>
		/// <returns>
		/// A new <see cref="BinaryWebResponse"/>.
		/// </returns>
		public static WebResponse CreateBinaryResponse( HttpWebResponse webResponse ) {
			return new BinaryWebResponse( true, webResponse );
		}

		/// <summary>
		/// Creates a <see cref="JavaScriptWebResponse"/>.
		/// </summary>
		/// <param name="webResponse">
		/// The original <see cref="HttpWebResponse"/>.
		/// </param>
		/// <returns>
		/// A new <see cref="JavaScriptWebResponse"/>.
		/// </returns>
		public static WebResponse CreateJavaScriptResponse( HttpWebResponse webResponse ) {
			return new JavaScriptWebResponse( true, webResponse );
		}

		/// <summary>
		/// Creates a <see cref="JsonWebResponse"/>.
		/// </summary>
		/// <param name="webResponse">
		/// The original <see cref="HttpWebResponse"/>.
		/// </param>
		/// <returns>
		/// A new <see cref="JsonWebResponse"/>.
		/// </returns>
		public static WebResponse CreateJsonResponse( HttpWebResponse webResponse ) {
			return new JsonWebResponse( true, webResponse );
		}

		/// <summary>
		/// Creates a <see cref="WebResponse"/> for an <see cref="HttpWebResponse"/>, based on its
		/// content type.
		/// </summary>
		/// <param name="webResponse">
		/// The <see cref="HttpWebResponse"/> to parse.
		/// </param>
		/// <returns>
		/// If the content type is registered with the <see cref="WebResponseFactory"/>, the corresponding
		/// <see cref="WebResponse"/> object; otherwise, an <see cref="UnsupportedWebResponse"/> object.
		/// </returns>
		public static WebResponse CreateResponse( HttpWebResponse webResponse ) {
			if ( !webResponse.Headers.AllKeys.Contains( CommonHeaders.ContentType ) ) {
				// The response is missing a content type.
				return new UnsupportedWebResponse( webResponse.ResponseUri, string.Empty );
			}

			var contentType = webResponse.Headers[CommonHeaders.ContentType];

			// The keys in the SupportedContentTypes indicate the text the contentType
			// must start with. Find that key
			var key = SupportedContentTypes.Keys.SingleOrDefault( k => contentType.StartsWith( k, StringComparison.OrdinalIgnoreCase ) );

			// We don't support this content type
			if ( key == null ) {
				return new UnsupportedWebResponse( webResponse.ResponseUri, contentType );
			}

			return SupportedContentTypes[key]( webResponse );
		}
	}
}
