using System.Net;
using Xunit;

namespace NScrape.Test;

public class WebClientTests {
	[Fact]
	public void WebClientProxyDefaultsToNull() {
		var webClient = new WebClient();

		Assert.Null( webClient.Proxy );
	}

	[Fact]
	public void WebClientProxyCanBeSet() {
		var proxy = new WebProxy( "http://proxy.example.com:8080" );

		var webClient = new WebClient {
			Proxy = proxy
		};

		Assert.Equal( proxy, webClient.Proxy );
	}

	[Fact]
	public void WebClientProxyCanBeSetToNull() {
		var webClient = new WebClient {
			Proxy = new WebProxy( "http://proxy.example.com:8080" )
		};

		webClient.Proxy = null;

		Assert.Null( webClient.Proxy );
	}
}
