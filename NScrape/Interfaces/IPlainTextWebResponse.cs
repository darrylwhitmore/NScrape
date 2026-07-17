namespace NScrape.Interfaces;

/// <summary>
/// Defines the contract for a plain text web response.
/// </summary>
public interface IPlainTextWebResponse : IWebResponse {
	/// <summary>
	/// Gets the plain text.
	/// </summary>
	string PlainText { get; }
}