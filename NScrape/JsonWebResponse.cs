using System;
using System.Text;

namespace NScrape
{
    /// <summary>
    /// Represents a web response for a request that returned JSON.
    /// </summary>
    public class JsonWebResponse : TextWebResponse
    {
        internal JsonWebResponse(bool success, Uri url, string text, Encoding encoding)
            : base(url, WebResponseType.JavaScript, success, text, encoding)
        {
        }

        /// <summary>
        /// Gets the JSON data.
        /// </summary>
        public string Json { get { return Text; } }
    }
}
