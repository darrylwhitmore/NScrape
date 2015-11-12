using System;
using System.Text;
using System.Xml.Linq;

namespace NScrape {
	/// <summary>
	/// Represents a web response for a request that returned XML.
	/// </summary>
	public class XmlWebResponse : TextWebResponse {

		public XmlWebResponse( bool success, Uri url, string xml, Encoding encoding )
			: base( url, WebResponseType.Xml, success, xml, encoding ) {
			XDocument = XDocument.Parse( xml );
		}

		/// <summary>
		/// Gets the XML.
		/// </summary>
		public XDocument XDocument { get; private set; }
	}
}
