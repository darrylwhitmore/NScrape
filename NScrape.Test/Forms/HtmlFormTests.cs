using System;
using System.Collections.Generic;
using System.Linq;
using NScrape.Forms;
using NScrape.Interfaces;
using NScrape.Requests;
using NScrape.Responses;
using NSubstitute;
using Xunit;

namespace NScrape.Test.Forms;

public class HtmlFormTests {

	[Fact]
	public void WeatherScrapeTest() {
		var webClient = Substitute.For<IWebClient>();

		var mockFormLoadResponse = Substitute.For<IHtmlWebResponse>();
		mockFormLoadResponse.Success.Returns( true );
		mockFormLoadResponse.ResponseType.Returns( WebResponseType.Html );
		mockFormLoadResponse.ResponseUrl.Returns( new Uri( "http://www.weather.gov/" ) );	
		mockFormLoadResponse.Html.Returns( Properties.Resources.weather_gov );
		webClient.SendRequest( Arg.Any<GetWebRequest>() ).Returns( mockFormLoadResponse );

		var mockFormSubmitResponse = Substitute.For<IHtmlWebResponse>();
		mockFormSubmitResponse.Success.Returns( true );
		mockFormSubmitResponse.ResponseType.Returns( WebResponseType.Html );
		mockFormSubmitResponse.ResponseUrl.Returns( new Uri( "https://forecast.weather.gov/MapClick.php?lat=21.30992&lon=-157.858158" ) );
		mockFormSubmitResponse.Html.Returns( Properties.Resources.weather_gov_honolulu_weather );
		webClient.SendRequest( Arg.Any<GetWebRequest>() ).Returns( mockFormSubmitResponse );


		var form = new BasicHtmlForm( webClient );
		form.Load( new Uri( "http://www.weather.gov/" ), new KeyValuePair<string, string>( "name", "getForecast" ) );
		form.InputControls.Single( c => c.Name == "inputstring" ).Value = "honolulu, hi";

		using var response = form.Submit();
		
		Assert.Equal( WebResponseType.Html, response.ResponseType );

		var scraper = new WeatherScraper( ( ( IHtmlWebResponse )response ).Html );

		var conditions = scraper.GetConditions();
		Assert.Equal( "Partly Cloudy", conditions );

		var temperature = scraper.GetTemperature();
		Assert.Equal( "81°F", temperature );
	}

	[Fact]
	public void BasicAspxTest() {
		var webClient = Substitute.For<IWebClient>();

		var mockFormLoadResponse = Substitute.For<IHtmlWebResponse>();
		mockFormLoadResponse.Success.Returns( true );
		mockFormLoadResponse.ResponseType.Returns( WebResponseType.Html );
		mockFormLoadResponse.ResponseUrl.Returns( new Uri( "https://www.cslb.ca.gov/onlineservices/checklicenseII/checklicense.aspx" ) );
		mockFormLoadResponse.Html.Returns( Properties.Resources.ca_gov_contractor_lookup );
		webClient.SendRequest( Arg.Any<GetWebRequest>() ).Returns( mockFormLoadResponse );

		var mockFormSubmitResponse = Substitute.For<IHtmlWebResponse>();
		mockFormSubmitResponse.Success.Returns( true );
		mockFormSubmitResponse.ResponseType.Returns( WebResponseType.Html );
		mockFormSubmitResponse.ResponseUrl.Returns( new Uri( "https://www.cslb.ca.gov/onlineservices/checklicenseII/LicenseDetail.aspx?LicNum=956855" ) );
		mockFormSubmitResponse.Html.Returns( Properties.Resources.ca_gov_contractor_license_lookup_results );
		webClient.SendRequest( Arg.Any<PostWebRequest>() ).Returns( mockFormSubmitResponse );
		
		
		var form = new BasicAspxForm( webClient );
		form.Load( new Uri( "https://www.cslb.ca.gov/onlineservices/checklicenseII/checklicense.aspx" ), new KeyValuePair<string, string>( "id", "ctl00" ) );
		form.InputControls.Single( c => c.Name == "ctl00$MainContent$LicNo" ).Value = "956855";

		using var response = form.Submit( "ctl00$MainContent$Contractor_License_Number_Search" );

		Assert.Equal( WebResponseType.Html, response.ResponseType );

		var scraper = new ContractorLicenseScraper( ( ( IHtmlWebResponse )response ).Html );

		var businessName = scraper.GetBusinessName();
		Assert.Equal( "ABC AIR CONDITIONING & HEATING", businessName );
	}
}

