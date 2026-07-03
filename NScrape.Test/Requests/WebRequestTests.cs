using System.Net;
using NScrape.Requests;
using Xunit;

namespace NScrape.Test.Requests;

public class WebRequestTests {
	[Fact]
	public void WebRequestProxyDefaultsToNull() {
		var getWebRequest = new GetWebRequest();

		Assert.Null( getWebRequest.Proxy );
	}

	[Fact]
	public void WebRequestProxyCanBeSet() {
		var proxy = new WebProxy( "http://proxy.example.com:8080" );

		var postWebRequest = new PostWebRequest {
			Proxy = proxy
		};

		Assert.Equal( proxy, postWebRequest.Proxy );
	}

	[Fact]
	public void WebRequestProxyCanBeSetToNull() {
		var getWebRequest = new GetWebRequest {
			Proxy = new WebProxy( "http://proxy.example.com:8080" )
		};

		getWebRequest.Proxy = null;

		Assert.Null( getWebRequest.Proxy );
	}
}
