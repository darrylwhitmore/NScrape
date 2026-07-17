namespace NScrape.Interfaces;

/// <summary>
/// Defines the contract for an HTML web response, extending the functionality of a general web response.
/// </summary>
public interface IHtmlWebResponse : IWebResponse {
	/// <summary>
	/// Gets the HTML.
	/// </summary>
	string Html { get; }
}