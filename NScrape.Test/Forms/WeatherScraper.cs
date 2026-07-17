using System.Linq;

namespace NScrape.Test.Forms;

public class WeatherScraper : Scraper {
	public WeatherScraper( string html ) : base( html ) {
	}

	public string GetConditions() {
		var node = HtmlDocument.DocumentNode.Descendants().SingleOrDefault( n => n.Attributes.Contains( "class" ) && n.Attributes["class"].Value == "myforecast-current" );

		if ( node != null ) {
			return node.InnerText;
		}

		throw new ScrapeException( "Could not scrape conditions.", Html );
	}

	public string GetTemperature() {
		var node = HtmlDocument.DocumentNode.Descendants().SingleOrDefault( n => n.Attributes.Contains( "class" ) && n.Attributes["class"].Value == "myforecast-current-lrg" );

		if ( node != null ) {
			return node.InnerText.Replace( "&deg;", "°" );
		}

		throw new ScrapeException( "Could not scrape temperature.", Html );
	}
}
