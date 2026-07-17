namespace NScrape.Interfaces;

/// <summary>
/// Defines the contract for a JSON-based web response.
/// </summary>
/// <remarks>
/// This interface extends <see cref="NScrape.Interfaces.IWebResponse"/> to include functionality specific to JSON responses.
/// </remarks>
public interface IJsonWebResponse : IWebResponse {
	/// <summary>
	/// Gets the JSON data.
	/// </summary>
	string Json { get; }
}