using System;
using System.Net;
using NScrape.Interfaces;
using NScrape.Responses;
using NSubstitute;
using Xunit;

namespace NScrape.Test;

public class WebResponseFactoryTests {
	[Fact]
	public void NullHttpWebResponseThrowsArgumentNullException() {
		var factory = new WebResponseFactory();

		Assert.Throws<ArgumentNullException>( () => factory.CreateResponse( null ) );
	}

	[Fact]
	public void UnknownContentTypeReturnsUnsupportedResponse() {
		var fakeHttpWebResponse = Substitute.For<HttpWebResponse>();
		fakeHttpWebResponse.Headers.Returns( new WebHeaderCollection { { CommonHeaders.ContentType, "text/unsupported" } } );
		fakeHttpWebResponse.ResponseUri.Returns( new Uri( "https://unsupported.com/" ) );

		var factory = new WebResponseFactory();
		var response = factory.CreateResponse( fakeHttpWebResponse );
		
		Assert.IsType<UnsupportedWebResponse>( response );
		Assert.Equal( "https://unsupported.com/", response.ResponseUrl.AbsoluteUri );
		Assert.Equal( "text/unsupported", ( (IUnsupportedWebResponse)response ).ContentType );
		
		Assert.Equal( WebResponseType.Unsupported, response.ResponseType );
		Assert.False( response.Success );
	}

	[Fact]
	public void HtmlContentTypeReturnsHtmlResponse() {
		var fakeHttpWebResponse = Substitute.For<HttpWebResponse>();
		fakeHttpWebResponse.Headers.Returns( new WebHeaderCollection { { CommonHeaders.ContentType, "text/html" } } );
		fakeHttpWebResponse.ResponseUri.Returns( new Uri( "https://html.com/" ) );
		
		var factory = new WebResponseFactory();
		var response = factory.CreateResponse( fakeHttpWebResponse );

		Assert.IsType<HtmlWebResponse>( response );
		Assert.Equal( "https://html.com/", response.ResponseUrl.AbsoluteUri );

		Assert.Equal( WebResponseType.Html, response.ResponseType );
		Assert.True( response.Success );
	}
}