using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using NScrape.Interfaces;
using NScrape.Responses;

namespace NScrape {
	/// <summary>
	/// Creates a <see cref="IWebResponse"/> object based on an <see cref="HttpWebResponse"/> object.
	/// </summary>
	public class WebResponseFactory {
		/// <summary>
		/// Initializes static members of the <see cref="WebResponseFactory"/> class.
		/// </summary>
		static WebResponseFactory() {
			SupportedContentTypes = new Dictionary<string, Func<HttpWebResponse, IWebResponse>> {
				{ "application/javascript", CreateJavaScriptResponse },
				{ "application/json", CreateJsonResponse },
				{ "application/octet-stream", CreateBinaryResponse },
				{ "application/pdf", CreateBinaryResponse },
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
		/// <see cref="Func{HttpWebResponse, IWebResponse}"/> that takes an <see cref="HttpWebResponse"/>
		/// object with the given content type and returns a <see cref="IWebResponse"/>. The return value
		/// is usually a subclass of the <see cref="IWebResponse"/> interface.
		/// </remarks>
		public static Dictionary<string, Func<HttpWebResponse, IWebResponse>> SupportedContentTypes { get; }

		/// <summary>
		/// Creates an <see cref="IHtmlWebResponse"/>.
		/// </summary>
		/// <param name="httpWebResponse">
		/// The original <see cref="HttpWebResponse"/>.
		/// </param>
		/// <returns>
		/// A new <see cref="IHtmlWebResponse"/>.
		/// </returns>
		public static IWebResponse CreateHtmlResponse( HttpWebResponse httpWebResponse ) {
			return new HtmlWebResponse( true, httpWebResponse );
		}

		/// <summary>
		/// Creates a <see cref="IImageWebResponse"/>.
		/// </summary>
		/// <param name="httpWebResponse">
		/// The original <see cref="HttpWebResponse"/>.
		/// </param>
		/// <returns>
		/// A new <see cref="IImageWebResponse"/>.
		/// </returns>
		public static IWebResponse CreateImageResponse( HttpWebResponse httpWebResponse ) {
			return new ImageWebResponse( true, httpWebResponse );
		}

		/// <summary>
		/// Creates a <see cref="IPlainTextWebResponse"/>.
		/// </summary>
		/// <param name="httpWebResponse">
		/// The original <see cref="HttpWebResponse"/>.
		/// </param>
		/// <returns>
		/// A new <see cref="IPlainTextWebResponse"/>.
		/// </returns>
		public static IWebResponse CreatePlainTextResponse( HttpWebResponse httpWebResponse ) {
			return new PlainTextWebResponse( true, httpWebResponse );
		}

		/// <summary>
		/// Creates an <see cref="IXmlWebResponse"/>.
		/// </summary>
		/// <param name="httpWebResponse">
		/// The original <see cref="HttpWebResponse"/>.
		/// </param>
		/// <returns>
		/// A new <see cref="IXmlWebResponse"/>.
		/// </returns>
		public static IWebResponse CreateXmlResponse( HttpWebResponse httpWebResponse ) {
			return new XmlWebResponse( true, httpWebResponse );
		}

		/// <summary>
		/// Creates a <see cref="IBinaryWebResponse"/>.
		/// </summary>
		/// <param name="httpWebResponse">
		/// The original <see cref="HttpWebResponse"/>.
		/// </param>
		/// <returns>
		/// A new <see cref="IBinaryWebResponse"/>.
		/// </returns>
		public static IWebResponse CreateBinaryResponse( HttpWebResponse httpWebResponse ) {
			return new BinaryWebResponse( true, httpWebResponse );
		}

		/// <summary>
		/// Creates a <see cref="IJavaScriptWebResponse"/>.
		/// </summary>
		/// <param name="httpWebResponse">
		/// The original <see cref="HttpWebResponse"/>.
		/// </param>
		/// <returns>
		/// A new <see cref="IJavaScriptWebResponse"/>.
		/// </returns>
		public static IWebResponse CreateJavaScriptResponse( HttpWebResponse httpWebResponse ) {
			return new JavaScriptWebResponse( true, httpWebResponse );
		}

		/// <summary>
		/// Creates a <see cref="IJsonWebResponse"/>.
		/// </summary>
		/// <param name="httpWebResponse">
		/// The original <see cref="HttpWebResponse"/>.
		/// </param>
		/// <returns>
		/// A new <see cref="IJsonWebResponse"/>.
		/// </returns>
		public static IWebResponse CreateJsonResponse( HttpWebResponse httpWebResponse ) {
			return new JsonWebResponse( true, httpWebResponse );
		}

		/// <summary>
		/// Creates a <see cref="IWebResponse"/> for an <see cref="HttpWebResponse"/>, based on its
		/// content type.
		/// </summary>
		/// <param name="httpWebResponse">
		/// The <see cref="HttpWebResponse"/> to parse.
		/// </param>
		/// <returns>
		/// If the content type is registered with the <see cref="WebResponseFactory"/>, the corresponding
		/// <see cref="IWebResponse"/> object; otherwise, an <see cref="UnsupportedWebResponse"/> object.
		/// </returns>
		public static IWebResponse CreateResponse( HttpWebResponse httpWebResponse ) {
			if ( !httpWebResponse.Headers.AllKeys.Contains( CommonHeaders.ContentType ) ) {
				// The response is missing a content type.
				return new UnsupportedWebResponse( httpWebResponse.ResponseUri, string.Empty );
			}

			var contentType = httpWebResponse.Headers[CommonHeaders.ContentType];

			// The keys in the SupportedContentTypes indicate the text the contentType
			// must start with. Find that key
			var key = SupportedContentTypes.Keys.SingleOrDefault( k => contentType.StartsWith( k, StringComparison.OrdinalIgnoreCase ) );

			// We don't support this content type
			if ( key == null ) {
				return new UnsupportedWebResponse( httpWebResponse.ResponseUri, contentType );
			}

			return SupportedContentTypes[key]( httpWebResponse );
		}
	}
}
