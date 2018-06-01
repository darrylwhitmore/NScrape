using HtmlAgilityPack;

namespace NScrape {
	/// <summary>
	/// Provides the base implementation for HTML scraper functionality.
	/// </summary>
	public abstract class Scraper {
		/// <summary>
		/// Initializes a new instance of the <see cref="Scraper"/> class.
		/// </summary>
		/// <param name="html">Contains the target HTML text.</param>
		/// <exception cref="ScrapeException">The target HTML text does not have a <b>DOCUMENT</b> node</exception>
		protected Scraper(string html) {
			HtmlDocument = new HtmlDocument();
			HtmlDocument.LoadHtml( html );

			if ( HtmlDocument.DocumentNode == null ) {
				throw new ScrapeException( "Document node is null.", html );
			}

			Html = html;
		}

		/// <summary>
		/// Gets an <see href="http://html-agility-pack.net/">Html Agility Pack</see>&#160;<b>HtmlDocument</b> loaded with the target HTML text.
		/// </summary>
		/// <remarks>
		/// Using the <see href="http://html-agility-pack.net/">Html Agility Pack</see> to scrape is recommended, but if you prefer, you can
		/// scrape using regular expressions or standard string functions by accessing the <see cref="Html"/> property.
		/// </remarks>
		protected HtmlDocument HtmlDocument { get; private set; }

		/// <summary>
		/// Gets the target HTML text.
		/// </summary>
		protected string Html { get; private set; }
	}
}
