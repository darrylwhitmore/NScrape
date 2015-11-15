using System;
using System.Text;
using System.Xml.Linq;

namespace NScrape {
	/// <summary>
	/// Represents a web response for a request that returned XML.
	/// </summary>
	public class XmlWebResponse : TextWebResponse {

		/// <summary>
		/// Initializes a new instance of the <see cref="XmlWebResponse"/> class.
		/// </summary>
		/// <param name="success"><b>true</b> if the response is considered successful, <b>false</b> otherwise.</param>
		/// <param name="responseUrl">The URL of the response.</param>
		/// <param name="xml">The XML text of the response.</param>
		/// <param name="encoding">The encoding of the XML text.</param>
		public XmlWebResponse( bool success, Uri responseUrl, string xml, Encoding encoding )
			: base( responseUrl, WebResponseType.Xml, success, xml, encoding ) {
			XDocument = XDocument.Parse( xml );
		}

		/// <summary>
		/// Gets the XML.
		/// </summary>
		public XDocument XDocument { get; private set; }
	}
}
