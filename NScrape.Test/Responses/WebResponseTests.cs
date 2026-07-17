using System;
using System.IO;
using System.Net;
using System.Text;
using NScrape.Responses;
using NSubstitute;
using Xunit;

namespace NScrape.Test.Responses;

public class WebResponseTests {
	[Fact]
	public void HtmlWebResponseExposesHtmlContent() {
		var responseUrl = new Uri( "https://example.com/" );
		var htmlContent = "<html><body>Hello</body></html>";

		var fakeHttpWebResponse = Substitute.For<HttpWebResponse>();
		fakeHttpWebResponse.ResponseUri.Returns( responseUrl );
		fakeHttpWebResponse.Headers.Returns( new WebHeaderCollection {
			{ "Content-Type", "text/html; charset=utf-8" }
		} );
		//fakeHttpWebResponse.CharacterSet.Returns( "utf-8" );  // TODO: add content assertions after HttpClient migration

		var stream = new MemoryStream( Encoding.UTF8.GetBytes( htmlContent ) );
		fakeHttpWebResponse.GetResponseStream().Returns( stream );

		using var response = new HtmlWebResponse( true, fakeHttpWebResponse );
		Assert.Equal( WebResponseType.Html, response.ResponseType );
		Assert.Equal( responseUrl, response.ResponseUrl );
		//Assert.Equal( htmlContent, response.Html );  // TODO: add content assertions after HttpClient migration
	}
}
