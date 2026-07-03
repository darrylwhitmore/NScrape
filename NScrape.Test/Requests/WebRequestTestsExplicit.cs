using System;
using System.Net;
using NScrape.Interfaces;
using NScrape.Requests;
using NScrape.Responses;
using Xunit;

namespace NScrape.Test.Requests;

[Trait( "Category", "Integration" )]
public class WebRequestTestsExplicit {
	private readonly ITestOutputHelper output;
	
	public WebRequestTestsExplicit( ITestOutputHelper output ) {
		this.output = output;
	}

	[Fact( Explicit = true )]
	public void WebRequestWithProxyReturnsExpectedResponse() {
		var uri = new Uri( "https://github.com/darrylwhitmore/NScrape" );

		var proxyProvider = new ProxyProvider();
		var candidateProxies = proxyProvider.GetProxies();

		var webClient = new WebClient();

		foreach ( var candidateProxy in candidateProxies ) {
			output.WriteLine( $"Testing proxy: {candidateProxy}" );
			
			var getRequest = new GetWebRequest( uri ) {
				Proxy = new WebProxy( candidateProxy )
			};

			using var response = webClient.SendRequest( getRequest );
			
			if ( response.Success ) {
				Assert.NotNull( response );
				Assert.True( response.Success );
				Assert.Equal( WebResponseType.Html, response.ResponseType );
				Assert.Equal( uri, response.ResponseUrl );

				var htmlWebResponse = response as IHtmlWebResponse;
				Assert.NotNull( htmlWebResponse );

				Assert.NotNull( htmlWebResponse.Html );
				Assert.Contains( "A web scraping framework for .NET", htmlWebResponse.Html );

				return;
			}
		}

		Assert.Fail( "No candidate proxies succeeded" );
	}
}
