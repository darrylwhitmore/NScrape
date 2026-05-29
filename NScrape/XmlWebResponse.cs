using System.Net;
using System.Xml.Linq;
using NScrape.Interfaces;

namespace NScrape {
	/// <summary>
	/// Represents a web response containing XML data.
	/// </summary>
	/// <remarks>
	/// This class extends <see cref="TextWebResponse"/> and implements <see cref="IXmlWebResponse"/>.
	/// It provides access to the XML content of the response through the <see cref="XDocument"/> property.
	/// </remarks>
	public class XmlWebResponse : TextWebResponse, IXmlWebResponse {

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
