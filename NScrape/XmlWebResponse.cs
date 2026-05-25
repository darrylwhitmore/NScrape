using System;
using System.Net;
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
		/// <param name="webResponse">The web response object.</param>
		public XmlWebResponse( bool success, HttpWebResponse webResponse )
			: base( success, WebResponseType.Xml, webResponse ) {
			XDocument = XDocument.Parse( Text );
		}

		/// <summary>
		/// Gets the XML.
		/// </summary>
		public XDocument XDocument { get; private set; }
	}
}
