using System;
using System.Linq;

namespace NScrape.Test.Forms;

internal class ContractorLicenseScraper : Scraper {
	public ContractorLicenseScraper( string html ) : base( html ) {
	}

	public string GetBusinessName() {
		var node = HtmlDocument.DocumentNode.Descendants().SingleOrDefault( n => n.Attributes.Contains( "id" ) && n.Attributes["id"].Value == "MainContent_BusInfo" );

		if ( node != null ) {
			var address = node.InnerHtml.Split( "<br>", StringSplitOptions.RemoveEmptyEntries );

			return address[0];
		}

		throw new ScrapeException( "Could not scrape business name.", Html );
	}
}