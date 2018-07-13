using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;

namespace NScrape {
	/// <summary>
	/// Provides miscellaneous extension methods.
	/// </summary>
	public static class NScrapeExtensions {
		/// <summary>
		/// Gets the encoding used by an <see cref="HttpWebResponse"/>.
		/// </summary>
		/// <param name="webResponse">
		/// The <see cref="HttpWebResponse"/> for which to determine the content type.
		/// </param>
		/// <remarks>
		/// If no encoding was specified, the <c>ISO-8859-1</c> character set is assumed.
		/// </remarks>
		/// <returns>
		/// The content type used by the <see cref="HttpWebResponse"/>.
		/// </returns>
		public static Encoding GetEncoding( this HttpWebResponse webResponse ) {
			// If a character set is not specified, RFC2616 section 3.7.1 says to use ISO-8859-1, per the page below.
			// The page says this is more or less useless, but I did find that Chrome and Firefox behaved this way
			// for javascript files that I tested.
			// http://www.w3.org/TR/html4/charset.html#h-5.2.2
			var characterSet = !string.IsNullOrEmpty( webResponse.CharacterSet ) ? webResponse.CharacterSet : "iso-8859-1";
			return Encoding.GetEncoding( characterSet );
		}

		/// <summary>
		/// Reads an <see cref="HttpWebResponse"/> as plain text.
		/// </summary>
		/// <param name="webResponse">
		/// The <see cref="HttpWebResponse"/> to read.
		/// </param>
		/// <param name="encoding">
		/// The encoding to be used. If omitted, the encoding specified in the web response shall be used.
		/// </param>
		/// <returns>
		/// A <see cref="string"/> that represents the text of an <see cref="HttpWebResponse"/>.
		/// </returns>
		public static string GetResponseText( this HttpWebResponse webResponse, Encoding encoding = null ) {
			if ( encoding == null ) {
				encoding = webResponse.GetEncoding();
			}

			var s = webResponse.GetResponseStream();

			if ( s != null ) {
				StreamReader sr;

				if ( webResponse.Headers[HttpResponseHeader.ContentEncoding] == "gzip" ) {
					sr = new StreamReader( new GZipStream( s, CompressionMode.Decompress ), encoding );
				}
				else {
					sr = new StreamReader( s, encoding );
				}

				var content = sr.ReadToEnd();

				sr.Dispose();

				return content;
			}

			return null;
		}
	}
}
