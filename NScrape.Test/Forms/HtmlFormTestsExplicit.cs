using System;
using System.Collections.Generic;
using System.Linq;
using NScrape.Forms;
using NScrape.Interfaces;
using NScrape.Responses;
using Xunit;

namespace NScrape.Test.Forms {
	
	[Trait( "Category", "Integration" )]
	public class HtmlFormTestsExplicit {
		private class MultipleControlsScraper : Scraper {
			public MultipleControlsScraper( string html ) : base( html ) {
			}

			public string GetUserName() {
				var node = HtmlDocument.DocumentNode.Descendants().SingleOrDefault( n => n.Attributes.Contains( "id" ) && n.Attributes["id"].Value == "_valueusername" );

				return node != null ? node.InnerText : throw new ScrapeException( "Could not scrape username.", Html );
			}

			public string GetPassword() {
				var node = HtmlDocument.DocumentNode.Descendants().SingleOrDefault( n => n.Attributes.Contains( "id" ) && n.Attributes["id"].Value == "_valuepassword" );

				return node != null ? node.InnerText : throw new ScrapeException( "Could not scrape password.", Html );
			}
			public string GetComments() {
				var node = HtmlDocument.DocumentNode.Descendants().SingleOrDefault( n => n.Attributes.Contains( "id" ) && n.Attributes["id"].Value == "_valuecomments" );

				return node != null ? node.InnerText : throw new ScrapeException( "Could not scrape comments.", Html );
			}
			public string GetHiddenField() {
				var node = HtmlDocument.DocumentNode.Descendants().SingleOrDefault( n => n.Attributes.Contains( "id" ) && n.Attributes["id"].Value == "_valuehiddenField" );

				return node != null ? node.InnerText : throw new ScrapeException( "Could not scrape hidden field.", Html );
			}

			public List<string> GetCheckboxes() {
				var nodes = HtmlDocument.DocumentNode.Descendants().Where( n => n.Attributes.Contains( "id" ) && n.Attributes["id"].Value.StartsWith( "_valuecheckboxes" ) );

				return nodes is null ? throw new ScrapeException( "Could not scrape checkboxes" ) : ( from node in nodes select node.InnerText ).ToList();
			}
			public string GetRadioButton() {
				var node = HtmlDocument.DocumentNode.Descendants().SingleOrDefault( n => n.Attributes.Contains( "id" ) && n.Attributes["id"].Value == "_valueradioval" );

				return node != null ? node.InnerText : throw new ScrapeException( "Could not scrape radio button.", Html );
			}

			public List<string> GetMultiSelect() {
				var nodes = HtmlDocument.DocumentNode.Descendants().Where( n => n.Attributes.Contains( "id" ) && n.Attributes["id"].Value.StartsWith( "_valuemultipleselect" ) );

				return nodes is null ? throw new ScrapeException( "Could not scrape multi-select options.", Html ) : ( from node in nodes select node.InnerText ).ToList();
			}

			public string GetDropDown() {
				var node = HtmlDocument.DocumentNode.Descendants().SingleOrDefault( n => n.Attributes.Contains( "id" ) && n.Attributes["id"].Value == "_valuedropdown" );

				return node != null ? node.InnerText : throw new ScrapeException( "Could not scrape dropdown.", Html );
			}
		}

		private class WeatherScraper : Scraper {
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



		[Fact( Explicit = true )]
		public void MultipleFormControlsTest() {
			var webClient = new WebClient();

			var form = new BasicHtmlForm( webClient );
			form.Load( new Uri( "https://testpages.eviltester.com/pages/forms/html-form/" ), new KeyValuePair<string, string>( "name", "HTMLFormElements" ) );
			form.InputControls.Single( c => c.Name == "username" ).Value = "theUser";

			form.InputControls.Single( c => c.Name == "password" ).Value = "123abc";

			form.TextAreaControls.Single( c => c.Name == "comments" ).Text = "no comment";

			form.InputControls.Single( c => c.Name == "hiddenField" ).Value = @"c:\directory\file";

			form.CheckBoxControls.Single( c => c.Value == "cb1" ).Checked = false;
			form.CheckBoxControls.Single( c => c.Value == "cb2" ).Checked = true;
			form.CheckBoxControls.Single( c => c.Value == "cb3" ).Checked = false;

			form.RadioControls.Single( c => c.Value == "rd1" ).Checked = false;
			form.RadioControls.Single( c => c.Value == "rd2" ).Checked = false;
			form.RadioControls.Single( c => c.Value == "rd3" ).Checked = true;

			form.SelectControls.Single( c => c.Name == "multipleselect[]" ).UnselectAll();
			form.SelectControls.Single( c => c.Name == "multipleselect[]" ).Options.Single( o => o.Value == "ms1" ).Selected = true;
			form.SelectControls.Single( c => c.Name == "multipleselect[]" ).Options.Single( o => o.Value == "ms3" ).Selected = true;

			form.SelectControls.Single( c => c.Name == "dropdown" ).UnselectAll();
			form.SelectControls.Single( c => c.Name == "dropdown" ).Options.Single( o => o.Value == "dd5" ).Selected = true;

			form.InputControls.Single( c => c.Name == "image" ).Disabled = true;

			form.InputControls.Single( c => c.Name == "submitbutton" && c.Value == "cancel" ).Disabled = true;

			using ( var response = form.Submit() ) {

				if ( response.ResponseType == WebResponseType.Html ) {
					var scraper = new MultipleControlsScraper( ( ( IHtmlWebResponse )response ).Html );

					Assert.Equal( "theUser", scraper.GetUserName() );

					Assert.Equal( "123abc", scraper.GetPassword() );

					Assert.Equal( "no comment", scraper.GetComments() );

					Assert.Equal( @"c:\directory\file", scraper.GetHiddenField() );

					// Should find our checkbox, but the site incorrectly requires the url encoding in the form
					// submission to be uppercase. The url encoding is supposed to be case-insensitive (ours is done by
					// .Net and is lowercase). 
					var scrapedCheckboxes = scraper.GetCheckboxes();
					Assert.Empty( scrapedCheckboxes );

					Assert.Equal( "rd3", scraper.GetRadioButton() );

					// Should find our multi selections, but the site incorrectly requires the url encoding in the form
					// submission to be uppercase. The url encoding is supposed to be case-insensitive (ours is done by
					// .Net and is lowercase). 
					var scrapedSelections = scraper.GetMultiSelect();
					Assert.Empty( scrapedSelections );

					Assert.Equal( "dd5", scraper.GetDropDown() );
				}
			}
		}

		[Fact( Explicit = true, Skip = "Site has changed to dynamically generate the values to be scraped" )]
		public void ScrapeTest() {
			var webClient = new WebClient();

			var form = new BasicHtmlForm( webClient );
			form.Load( new Uri( "http://www.weather.gov/" ), new KeyValuePair<string, string>( "name", "getForecast" ) );
			form.InputControls.Single( c => c.Name == "inputstring" ).Value = "fairbanks, ak";

			using ( var response = form.Submit() ) {

				if ( response.ResponseType == WebResponseType.Html ) {
					var scraper = new WeatherScraper( ( ( IHtmlWebResponse )response ).Html );

					var conditions = scraper.GetConditions();

					var temperature = scraper.GetTemperature();
				}
			}
		}

		[Fact( Explicit = true, Skip = "A bunch of sample form/control code without any tests" )]
		public void BasicHtmlTest() {
			var webClient = new WebClient();

			//var form = new BasicHtmlForm( webClient );
			//form.Load( new Uri( "http://chicago.craigslist.org/search/ela" ), new KeyValuePair<string, string>( "id", "searchform" ) );
			//form.InputControls.Single( c => c.Name == "query" ).Value = "xbox";
			//form.InputControls.Single( c => c.Name == "min_price" ).Value = "0";
			//form.InputControls.Single( c => c.Name == "max_price" ).Value = "200";
			//form.CheckBoxControls.Single( c => c.Name == "srchType" ).Checked = true;
			//using ( var response = form.Submit() ) {
			//}

			//var form = new BasicHtmlForm( webClient );
			//form.Load( new Uri( "http://chicago.craigslist.org/" ), new KeyValuePair<string, string>( "id", "search" ) );
			//var options = form.SelectControls.Single( c => c.Name == "catAbb" ).Options;
			//var selected = options.Where( o => o.Selected );
			//foreach ( var option in selected ) {
			//	option.Selected = false;
			//}
			//options.Single( o => o.Value == "ppp" ).Selected = true;
			//form.InputControls.Single( c => c.Name == "query" ).Value = "iphone";
			//using ( var response = form.Submit() ) {
			//}

			//var form = new BasicHtmlForm( webClient );
			//form.Load( new Uri( "http://www.timeanddate.com/" ), new KeyValuePair<string, string>( "id", "cf" ) );
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
			//options.First( o => o.Option == "United States" ).Selected = true;
			//selected = options.Where( o => o.Selected );
			//using ( var response = form.Submit() ) {
			//}

			//var form = new BasicHtmlForm( webClient );
			//form.Load( new Uri( "http://www.quackit.com/html/codes/comment_box_code.cfm" ), 2 );
			//form.TextAreaControls.Single( c => c.Name == "comments" ).Text = "It works!!!!!!!!!!!!!!!!!!";
			//using ( var response = form.Submit() ) {
			//}
		}

		[Fact( Explicit = true )]
		public void BasicAspxTest() {
			var webClient = new WebClient();

			var form = new BasicAspxForm( webClient );
			form.Load( new Uri( "https://www.cslb.ca.gov/onlineservices/checklicenseII/checklicense.aspx" ), new KeyValuePair<string, string>( "id", "ctl00" ) );
			form.InputControls.Single( c => c.Name == "ctl00$MainContent$LicNo" ).Value = "999";

			using ( var response = form.Submit( "ctl00$MainContent$Contractor_License_Number_Search" ) ) {
			}
		}

		[Fact( Explicit = true, Skip = "Broken, but do we need a second ASPX test anyway?" )]
		public void BasicAspxTest2() {
			var webClient = new WebClient();

			var form = new BasicAspxForm( webClient );
			form.Load( new Uri( "http://www.ncsc.org/information-and-resources/browse-by-state/state-court-websites.aspx" ), new KeyValuePair<string, string>( "name", "ctl01" ) );
			form.InputControls.Single( c => c.Name == "header_0$ctl02$txtSearch" ).Value = "fubar";

			using ( var response = form.Submit( "header_0$ctl02$btnSearch" ) ) {
			}
		}
	}
}
