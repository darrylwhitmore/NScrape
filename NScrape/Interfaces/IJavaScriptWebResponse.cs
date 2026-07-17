namespace NScrape.Interfaces;

/// <summary>
/// Represents a contract for a web response that contains JavaScript content.
/// </summary>
public interface IJavaScriptWebResponse : IWebResponse {
	/// <summary>
	/// Gets the JavaScript.
	/// </summary>
	string JavaScript { get; }
}