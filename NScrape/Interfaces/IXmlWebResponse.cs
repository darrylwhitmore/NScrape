using System.Xml.Linq;

namespace NScrape.Interfaces;

/// <summary>
/// Defines the contract for a web response that contains XML data.
/// </summary>
public interface IXmlWebResponse : IWebResponse {
	/// <summary>
	/// Gets the XML.
	/// </summary>
	XDocument XDocument { get; }
}