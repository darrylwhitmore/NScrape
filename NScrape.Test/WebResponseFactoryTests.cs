using System;
using System.Net;
using NScrape.ContentTypeHandlers;
using NScrape.Interfaces;
using NScrape.Responses;
using NSubstitute;
using Xunit;

namespace NScrape.Test;

public class WebResponseFactoryTests {
	private class CustomUnsupportedContentTypeHandler : IContentTypeHandler {
		public static readonly string ContentType = "text/unsupported";

		public string ContentTypePrefix => ContentType;
		
		public IWebResponse CreateResponse( HttpWebResponse httpWebResponse ) => new PlainTextWebResponse( true, httpWebResponse );
	}

	private class CustomSupportedContentTypeHandler : IContentTypeHandler {
		public static readonly string ContentType = "text/html";

		public string ContentTypePrefix => ContentType;

		public IWebResponse CreateResponse( HttpWebResponse httpWebResponse ) => new PlainTextWebResponse( true, httpWebResponse );
	}


	[Fact]
	public void NullHttpWebResponseCreatingResponseUsingHandlerThrowsArgumentNullException() {
		var handler = new HtmlContentTypeHandler("text/html");

		Assert.Throws<ArgumentNullException>( () => handler.CreateResponse( null ) );
	}
	
	[Fact]
	public void NullHttpWebResponseUsingFactoryThrowsArgumentNullException() {
		var factory = new WebResponseFactory();

		Assert.Throws<ArgumentNullException>( () => factory.CreateResponse( null ) );
	}

	[Fact]
	public void UnknownContentTypeReturnsUnsupportedResponse() {
		var responseUrl = new Uri( "https://unsupported.com/" );

		var fakeHttpWebResponse = Substitute.For<HttpWebResponse>();
		fakeHttpWebResponse.Headers.Returns( new WebHeaderCollection { { CommonHeaders.ContentType, "text/unsupported" } } );
		fakeHttpWebResponse.ResponseUri.Returns( responseUrl );

		var factory = new WebResponseFactory();
		var response = factory.CreateResponse( fakeHttpWebResponse );

		Assert.IsType<IUnsupportedWebResponse>( response, exactMatch: false );
		Assert.Equal( responseUrl, response.ResponseUrl );
		Assert.Equal( "text/unsupported", ( (IUnsupportedWebResponse)response ).ContentType );
		
		Assert.Equal( WebResponseType.Unsupported, response.ResponseType );
		Assert.False( response.Success );
	}

	[Fact]
	public void HtmlContentTypeReturnsHtmlResponse() {
		var responseUrl = new Uri( "https://html.com/" );

		var fakeHttpWebResponse = Substitute.For<HttpWebResponse>();
		fakeHttpWebResponse.Headers.Returns( new WebHeaderCollection { { CommonHeaders.ContentType, "text/html" } } );
		fakeHttpWebResponse.ResponseUri.Returns( responseUrl );

		var factory = new WebResponseFactory();
		var response = factory.CreateResponse( fakeHttpWebResponse );

		Assert.IsType<IHtmlWebResponse>( response, exactMatch: false );
		Assert.Equal( responseUrl, response.ResponseUrl );
	}

	[Fact]
	public void CustomUnsupportedContentTypeReturnsCustomResponse() {
		var fakeHttpWebResponse = Substitute.For<HttpWebResponse>();
		fakeHttpWebResponse.Headers.Returns( new WebHeaderCollection { { CommonHeaders.ContentType, CustomUnsupportedContentTypeHandler.ContentType } } );

		var factory = new WebResponseFactory( [
			new CustomUnsupportedContentTypeHandler()
		] );

		var response = factory.CreateResponse( fakeHttpWebResponse );

		Assert.IsType<IPlainTextWebResponse>( response, exactMatch: false );
	}

	[Fact]
	public void CustomSupportedContentTypeOverridesDefaultResponse() {
		var fakeHttpWebResponse = Substitute.For<HttpWebResponse>();
		fakeHttpWebResponse.Headers.Returns( new WebHeaderCollection { { CommonHeaders.ContentType, CustomSupportedContentTypeHandler.ContentType } } );

		var factory = new WebResponseFactory( [
			new CustomSupportedContentTypeHandler()
		] );

		var response = factory.CreateResponse( fakeHttpWebResponse );

		Assert.IsType<IPlainTextWebResponse>( response, exactMatch: false );
	}
}
