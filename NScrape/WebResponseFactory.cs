using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;

namespace NScrape
{
    /// <summary>
    /// Creates a <see cref="WebResponse"/> object based on a <see cref="HttpWebResponse"/> object.
    /// </summary>
    public class WebResponseFactory
    {
        /// <summary>
        /// Initializes static members of the <see cref="WebResponseFactory"/> class.
        /// </summary>
        static WebResponseFactory()
        {
            SupportedContentTypes.Add("image/", CreateImageResponse);
            SupportedContentTypes.Add("text/xml", CreateXmlResponse);
            SupportedContentTypes.Add("application/xml", CreateXmlResponse);
            SupportedContentTypes.Add("text/plain", CreateTextResponse);
            SupportedContentTypes.Add("text/javascript", CreateJavaScriptResponse);
            SupportedContentTypes.Add("application/javascript ", CreateJavaScriptResponse);
            SupportedContentTypes.Add("application/x-javascript", CreateJavaScriptResponse);
            SupportedContentTypes.Add("application/json", CreateJsonResponse);

            SupportedContentTypes = new Dictionary<string, Func<HttpWebResponse, WebResponse>>();
        }

        /// <summary>
        /// Gets a dictionary that contains all content types that are currently suported by the
        /// <see cref="WebResponseFactory"/>. The key of the dictionary is the text the content type
        /// must start with (but not necessarily the full text of the content type); the value is a
        /// <see cref="Func{HttpWebResponse, WebResponse}"/> that takes a <see cref="HttpWebResponse"/>
        /// object with the given content type and returns a <see cref="WebResponse"/>. The return value
        /// is usually a subclass of the <see cref="WebResponse"/> class.
        /// </summary>
        public static Dictionary<string, Func<HttpWebResponse, WebResponse>> SupportedContentTypes {
            get;
            private set;
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
        public static WebResponse CreateImageResponse(HttpWebResponse webResponse)
        {
            var image = new Bitmap(0, 0);

            var s = webResponse.GetResponseStream();

            if (s != null)
            {
                image = new Bitmap(s);
            }

            return new ImageWebResponse(true, webResponse.ResponseUri, image);
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
        public static WebResponse CreateTextResponse(HttpWebResponse webResponse)
        {
            var encoding = GetEncoding(webResponse);
            var text = ReadResponseText(webResponse, encoding);
            return new PlainTextWebResponse(true, webResponse.ResponseUri, text, encoding);
        }

        /// <summary>
        /// Creates a <see cref="XmlWebResponse"/>.
        /// </summary>
        /// <param name="webResponse">
        /// The original <see cref="HttpWebResponse"/>.
        /// </param>
        /// <returns>
        /// A new <see cref="XmlWebResponse"/>.
        /// </returns>
        public static WebResponse CreateXmlResponse(HttpWebResponse webResponse)
        {
            var encoding = GetEncoding(webResponse);
            var xml = ReadResponseText(webResponse, encoding);
            return new XmlWebResponse(true, webResponse.ResponseUri, xml, encoding);
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
        public static WebResponse CreateJavaScriptResponse(HttpWebResponse webResponse)
        {
            var encoding = GetEncoding(webResponse);
            var javaScript = ReadResponseText(webResponse, encoding);
            return new JavaScriptWebResponse(true, webResponse.ResponseUri, javaScript, encoding);
        }

        /// <summary>
        /// Creates a <see cref="CreateJsonResponse"/>.
        /// </summary>
        /// <param name="webResponse">
        /// The original <see cref="HttpWebResponse"/>.
        /// </param>
        /// <returns>
        /// A new <see cref="CreateJsonResponse"/>.
        /// </returns>
        public static WebResponse CreateJsonResponse(HttpWebResponse webResponse)
        {
            var encoding = GetEncoding(webResponse);
            var text = ReadResponseText(webResponse, encoding);
            return new JsonWebResponse(true, webResponse.ResponseUri, text, encoding);
        }

        /// <summary>
        /// Creates a <see cref="WebResponse"/> for a <see cref="HttpWebResponse"/>, based on its
        /// content type. If the content type is not registered with the <see cref="WebResponseFactory"/>,
        /// returns <see langword="null"/>.
        /// </summary>
        /// <param name="webResponse">
        /// The <see cref="HttpWebResponse"/> to parse.
        /// </param>
        /// <param name="contentType">
        /// The content type of the web response.
        /// </param>
        /// <returns>
        /// If the content type is registered with the <see cref="WebResponseFactory"/>, a new
        /// <see cref="WebResponse"/> object. Else, <see langword="null"/>.
        /// </returns>
        public static WebResponse CreateResponse(HttpWebResponse webResponse, string contentType)
        {
            // The keys in the SupportedContentTypes indicate the text the contentType
            // must start with. Find that key
            var key = SupportedContentTypes.Keys.SingleOrDefault(k => contentType.StartsWith(k, StringComparison.OrdinalIgnoreCase));

            // We don't support this content type
            if (key == null)
            {
                return null;
            }
            else
            {
                return SupportedContentTypes[key](webResponse);
            }
        }

        /// <summary>
        /// Gets the encoding used by a <see cref="HttpWebResponse"/>. Falls back to the <c>iso-8859-1</c>
        /// character set if no encoding was specified.
        /// </summary>
        /// <param name="webResponse">
        /// The <see cref="HttpWebResponse"/> for which to determine the content type.
        /// </param>
        /// <returns>
        /// The content type used by the <see cref="HttpWebResponse"/>.
        /// </returns>
        public static Encoding GetEncoding(HttpWebResponse webResponse)
        {
            // If a character set is not specified, RFC2616 section 3.7.1 says to use ISO-8859-1, per the page below.
            // The page says this is more or less useless, but I did find that Chrome and Firefox behaved this way
            // for javascript files that I tested.
            // http://www.w3.org/TR/html4/charset.html#h-5.2.2
            var characterSet = (!string.IsNullOrEmpty(webResponse.CharacterSet) ? webResponse.CharacterSet : "iso-8859-1");
            return Encoding.GetEncoding(characterSet);
        }

        /// <summary>
        /// Reads a <see cref="HttpWebResponse"/> as plain text.
        /// </summary>
        /// <param name="webResponse">
        /// The <see cref="HttpWebResponse"/> to read.
        /// </param>
        /// <returns>
        /// A <see cref="string"/> that represents the text of a <see cref="HttpWebResponse"/>.
        /// </returns>
        public static string ReadResponseText(HttpWebResponse webResponse)
        {
            var encoding = GetEncoding(webResponse);
            return ReadResponseText(webResponse, encoding);
        }

        /// <summary>
        /// Reads a <see cref="HttpWebResponse"/> as plain text.
        /// </summary>
        /// <param name="webResponse">
        /// The <see cref="HttpWebResponse"/> to read.
        /// </param>
        /// <param name="encoding">
        /// The encoding used.
        /// </param>
        /// <returns>
        /// A <see cref="string"/> that represents the text of a <see cref="HttpWebResponse"/>.
        /// </returns>
        public static string ReadResponseText(HttpWebResponse webResponse, Encoding encoding)
        {
            var s = webResponse.GetResponseStream();

            if (s != null)
            {
                StreamReader sr;

                if (webResponse.ContentEncoding == "gzip")
                {
                    sr = new StreamReader(new GZipStream(s, CompressionMode.Decompress), encoding);
                }
                else
                {
                    sr = new StreamReader(s, encoding);
                }

                var content = sr.ReadToEnd();

                sr.Close();

                return content;
            }

            return null;
        }
    }
}
