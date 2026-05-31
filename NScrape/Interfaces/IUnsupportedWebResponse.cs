namespace NScrape.Interfaces;

/// <summary>
/// Defines an interface for web responses that contain unsupported content types.
/// </summary>
public interface IUnsupportedWebResponse : IWebResponse {
	/// <summary>
	/// Gets the content type.
	/// </summary>
	string ContentType { get; }
}