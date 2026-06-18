using System;

namespace NScrape;

/// <summary>
/// The exception that is thrown when a web scraping operation fails.
/// </summary>
[Serializable]
public class ScrapeException : Exception {
	/// <summary>
	/// Initializes a new instance of the <see cref="ScrapeException"/> class.
	/// </summary>
	public ScrapeException() {
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ScrapeException"/> class with a specified error message.
	/// </summary>
	/// <param name="message">The error message that explains the reason for the exception.</param>
	public ScrapeException( string message )
		: base( message ) {
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ScrapeException"/> class with a specified error message and the HTML in question.
	/// </summary>
	/// <param name="message">The error message that explains the reason for the exception.</param>
	/// <param name="html">Contains the HTML text that could not be scraped.</param>
	public ScrapeException( string message, string html )
		: base( message ) {
		Html = html;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ScrapeException"/> class with a specified error message, the HTML in question and a reference to the inner exception that is the cause of this exception.
	/// </summary>
	/// <param name="message">The error message that explains the reason for the exception.</param>
	/// <param name="html">Contains the HTML text that could not be scraped.</param>
	/// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
	public ScrapeException( string message, string html, Exception innerException )
		: base( message, innerException ) {
		Html = html;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ScrapeException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
	/// </summary>
	/// <param name="message">The error message that explains the reason for the exception.</param>
	/// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
	public ScrapeException( string message, Exception innerException )
		: base( message, innerException ) {
	}

	/// <summary>
	/// Gets the HTML text that could not be scraped.
	/// </summary>
	public string Html { get; private set; }
}