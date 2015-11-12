using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using NScrape.Forms;

namespace NScrape.Test {
	public class HtmlFormTests {
		private class TestScraper : Scraper {
			public TestScraper( string html ) : base( html ) {
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

		[Fact]
		public void ScrapeTest() {
			var webClient = new WebClient();

			var form = new BasicHtmlForm( webClient );
			form.Load( new Uri( "http://www.weather.gov/" ), new KeyValuePair<string, string>( "name", "getForecast" ) );
			form.InputControls.Single( c => c.Name == "inputstring" ).Value = "fairbanks, ak";

			var response = form.Submit();

			if ( response.ResponseType == WebResponseType.Html ) {
				var scraper = new TestScraper( ( ( HtmlWebResponse )response ).Html );

				var conditions = scraper.GetConditions();

				var temperature = scraper.GetTemperature();
			}
		}

		[Fact]
		public void BasicHtmlTest() {
			var webClient = new WebClient();

			//var form = new BasicHtmlForm( webClient );
			//form.Load( new Uri( "http://chicago.craigslist.org/search/ela" ), "id", "searchform" );
			//form.InputControls.Single( c => c.Name == "query" ).Value = "xbox";
			//form.InputControls.Single( c => c.Name == "min_price" ).Value = "0";
			//form.InputControls.Single( c => c.Name == "max_price" ).Value = "200";
			//form.CheckBoxControls.Single( c => c.Name == "srchType" ).Checked = true;
			//var response = form.Submit();


			//var form = new BasicHtmlForm( webClient );
			//form.Load( new Uri( "http://chicago.craigslist.org/" ), "id", "search" );
			//var options = form.SelectControls.Single( c => c.Name == "catAbb" ).Options;
			//var selected = options.Where( o => o.Selected );
			//foreach ( var option in selected ) {
			//	option.Selected = false;
			//}
			//options.Single( o => o.Value == "ppp" ).Selected = true;
			//form.InputControls.Single( c => c.Name == "query" ).Value = "iphone";
			//var response = form.Submit();


			//var form = new BasicHtmlForm( webClient );
			//form.Load( new Uri( "http://www.timeanddate.com/" ), "id", "cf" );
			//form.InputControls.Single( c => c.Name == "year" ).Value = "1960";
			//foreach ( var c in form.RadioControls.Where( c => c.Name == "typ" ) ) {
			//	c.Checked = false;
			//}
			//form.RadioControls.Single( c => c.Value == "1" ).Checked = true;
			//form.SelectControls.Single( c => c.Name == "month" ).UnselectAll();
			//var options = form.SelectControls.Single( c => c.Name == "month" ).Options;
			//var selected = options.Where( o => o.Selected );
			//options.Single( o => o.Option == "May" ).Selected = true;
			//selected = options.Where( o => o.Selected );
			//options = form.SelectControls.Single( c => c.Name == "country" ).Options;
			//form.SelectControls.Single( c => c.Name == "country" ).UnselectAll();
			//selected = options.Where( o => o.Selected );
			//options.Single( o => o.Option == "United States" ).Selected = true;
			//selected = options.Where( o => o.Selected );
			//var response = form.Submit();

			//var form = new BasicHtmlForm( webClient );
			//form.Load( new Uri( "http://www.quackit.com/html/codes/comment_box_code.cfm" ), 1 );
			//form.TextAreaControls.Single( c => c.Name == "comments" ).Text = "It works!!!!!!!!!!!!!!!!!!";
			//var response = form.Submit();
		}

		[Fact]
		public void BasicAspxTest() {
			var webClient = new WebClient();

			var form = new BasicAspxForm( webClient );
			form.Load( new Uri( "http://architectfinder.aia.org/frmSearch.aspx" ), new KeyValuePair<string, string>( "name", "aspnetForm" ) );
			form.InputControls.Single( c => c.Name == "ctl00$ContentPlaceHolder1$txtCity" ).Value = "Boston";

			form.SelectControls.Single( c => c.Name == "ctl00$ContentPlaceHolder1$drpState" ).UnselectAll();
			var options = form.SelectControls.Single( c => c.Name == "ctl00$ContentPlaceHolder1$drpState" ).Options;
			options.Single( o => o.Option == "Massachusetts" ).Selected = true;

			var response = form.Submit( "ctl00$ContentPlaceHolder1$btnSearch" );
		}

		[Fact]
		public void BasicAspxTest2() {
			var webClient = new WebClient();

			var form = new BasicAspxForm( webClient );
			form.Load( new Uri( "http://www.chilis.com/EN/Pages/locationsearch.aspx" ), new KeyValuePair<string, string>( "name", "aspnetForm" ) );
			form.InputControls.Single( c => c.Name == "ctl00$PlaceHolderMain$LocationSearchBar$txtChilisLocator" ).Value = "Chicago, IL";

			var response = form.Submit( "ctl00$PlaceHolderMain$LocationSearchBar$ibtnChilisLocator" );
		}
	}
}
