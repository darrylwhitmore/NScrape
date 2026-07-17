using System.Net;

namespace NScrape.Interfaces;

/// <summary>
/// Provides a factory for creating <see cref="IWebResponse"/> instances from <see cref="HttpWebResponse"/> objects.
/// </summary>
public interface IWebResponseFactory {
	/// <summary>
	/// Creates an <see cref="IWebResponse"/> instance from the specified <see cref="HttpWebResponse"/> object.
	/// </summary>
	/// <param name="httpWebResponse">The <see cref="HttpWebResponse"/> object to create the <see cref="IWebResponse"/> from.</param>
	/// <returns>An <see cref="IWebResponse"/> instance representing the specified <see cref="HttpWebResponse"/>.</returns>
	IWebResponse CreateResponse( HttpWebResponse httpWebResponse );
}