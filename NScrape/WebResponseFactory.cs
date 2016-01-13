using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

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
				{ "text/javascript", CreateJavaScriptResponse },
				{ "text/plain", CreatePlainTextResponse },
				{ "text/xml", CreateXmlResponse }
			};
		}

		/// <summary>
		/// Gets a dictionary containing all content types currently suported by the
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
		public static Dictionary<string, Func<HttpWebResponse, WebResponse>> SupportedContentTypes { get; private set; }

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
		/// Creates a <see cref="PlainTextWebResponse"/>.
		/// </summary>
		/// <param name="webResponse">
		/// The original <see cref="HttpWebResponse"/>.
		/// </param>
		/// <returns>
		/// A new <see cref="PlainTextWebResponse"/>.
		/// </returns>
		/// <remarks>
		/// Deprecated; please use <see cref="CreatePlainTextResponse( HttpWebResponse )"/> instead.
		/// </remarks>
		[Obsolete( "Please use CreatePlainTextResponse( HttpWebResponse ) instead." )]
		public static WebResponse CreateTextResponse( HttpWebResponse webResponse ) {
			return CreatePlainTextResponse( webResponse );
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
		/// If the content type is registered with the <see cref="WebResponseFactory"/>, a new
		/// <see cref="WebResponse"/> object, otherwise, <see langword="null"/>.
		/// </returns>
		public static WebResponse CreateResponse( HttpWebResponse webResponse ) {
			var contentType = webResponse.Headers[CommonHeaders.ContentType];

			// The keys in the SupportedContentTypes indicate the text the contentType
			// must start with. Find that key
			var key = SupportedContentTypes.Keys.SingleOrDefault( k => contentType.StartsWith( k, StringComparison.OrdinalIgnoreCase ) );

			// We don't support this content type
			if ( key == null ) {
				return null;
			}

			return SupportedContentTypes[key]( webResponse );
		}

		/// <summary>
		/// Gets the encoding used by an <see cref="HttpWebResponse"/>.
		/// </summary>
		/// <param name="webResponse">
		/// The <see cref="HttpWebResponse"/> for which to determine the content type.
		/// </param>
		/// <returns>
		/// The content type used by the <see cref="HttpWebResponse"/>.
		/// </returns>
		/// <remarks>
		/// Deprecated; please use <see cref="NScrapeExtensions.GetEncoding( HttpWebResponse )"/> instead.
		/// </remarks>
		[Obsolete( "Please use NScrapeExtensions.GetEncoding( HttpWebResponse ) instead." )]
		public static Encoding GetEncoding( HttpWebResponse webResponse ) {
			return webResponse.GetEncoding();
		}

		/// <summary>
		/// Reads an <see cref="HttpWebResponse"/> as plain text.
		/// </summary>
		/// <param name="webResponse">
		/// The <see cref="HttpWebResponse"/> to read.
		/// </param>
		/// <returns>
		/// A <see cref="string"/> that represents the text of an <see cref="HttpWebResponse"/>.
		/// </returns>
		/// <remarks>
		/// Deprecated; please use <see cref="NScrapeExtensions.GetResponseText( HttpWebResponse, Encoding )"/> instead.
		/// </remarks>
		[Obsolete( "Please use NScrapeExtensions.GetResponseText( HttpWebResponse ) instead." )]
		public static string ReadResponseText( HttpWebResponse webResponse ) {
			return webResponse.GetResponseText();
		}

		/// <summary>
		/// Reads an <see cref="HttpWebResponse"/> as plain text.
		/// </summary>
		/// <param name="webResponse">
		/// The <see cref="HttpWebResponse"/> to read.
		/// </param>
		/// <param name="encoding">
		/// The encoding used.
		/// </param>
		/// <returns>
		/// A <see cref="string"/> that represents the text of an <see cref="HttpWebResponse"/>.
		/// </returns>
		/// <remarks>
		/// Deprecated; please use <see cref="NScrapeExtensions.GetResponseText( HttpWebResponse, Encoding )"/> instead.
		/// </remarks>
		[Obsolete( "Please use NScrapeExtensions.GetResponseText( HttpWebResponse, Encoding ) instead." )]
		public static string ReadResponseText( HttpWebResponse webResponse, Encoding encoding ) {
			return webResponse.GetResponseText( encoding );
		}
	}
}
