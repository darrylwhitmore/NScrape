using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NScrape.Interfaces;
using Xunit;

namespace NScrape.Test;

[Trait( "Category", "Integration" )]
public class ExampleTestsExplicit {
	public class BookstoreScraper : Scraper {
		public BookstoreScraper( string html ) : base( html ) { }

		public List<string> GetTitles() {
			return HtmlDocument.DocumentNode
				.Descendants( "article" )
				.Where( n => n.Attributes["class"]?.Value == "product_pod" )
				.Select( n => n.SelectSingleNode( ".//h3/a" ).Attributes["title"].Value )
				.ToList();
		}
	}

	[Fact( Explicit = true )]
	public void ReadMeCodeExampleWorksAsExpected() {
		var webClient = new WebClient();

		using var response = webClient.SendRequest( new Uri( "https://books.toscrape.com" ) );

		if ( response is IHtmlWebResponse htmlResponse ) {
			var scraper = new BookstoreScraper( htmlResponse.Html );
			var titles = scraper.GetTitles();

			Assert.Equal( 20, titles.Count );
			Assert.Equal( "A Light in the Attic", HttpUtility.HtmlDecode( titles[0] ) );
			Assert.Equal( "It's Only the Himalayas", HttpUtility.HtmlDecode( titles[19] ) );
		}
		else {
			Assert.Fail( "Failed to retrieve HTML response" );
		}
	}
	
}
