using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using NScrape.Interfaces;
using NScrape.Responses;

namespace NScrape.Test;

internal class ProxyProvider {
	// The proxy list is returned as text/html, so we use a plain text content type handler.
	private class TextProxyListContentTypeHandler : IContentTypeHandler {
		private const string ContentType = "text/html";

		public string ContentTypePrefix => ContentType;

		public IWebResponse CreateResponse( HttpWebResponse httpWebResponse ) => new PlainTextWebResponse( true, httpWebResponse );
	}

	// ProxyScrape Free Proxy List
	//  These are unpredictable and not guaranteed to work.
	//  https://proxyscrape.com/free-proxy-list
	//
	// Download with the following specifications:
	// - protocol: http
	// - protocol://ip:port
	// - country: us
	// - text format (still returned as text/html)
	const string FreeProxiesUriString = "https://api.proxyscrape.com/v4/free-proxy-list/get?request=display_proxies&proxy_format=protocolipport&format=text&protocol=http&country=us";

	public List<Uri> GetProxies() {
		var factory = new WebResponseFactory( [
			new TextProxyListContentTypeHandler()
		] );

		var webClient = new WebClient( factory );

		using var response = webClient.SendRequest( new Uri( FreeProxiesUriString ) );

		if ( !response.Success ) {
			throw new Exception( "Unsuccessful response attempting to get proxy list" );
		}

		if ( response.ResponseType != WebResponseType.PlainText ) {
			throw new Exception( $"Unexpected response type attempting to get proxy list: {response.ResponseType}" );
		}

		var plainTextResponse = ( PlainTextWebResponse )response;

		return plainTextResponse.PlainText.Split( ["\r\n", "\n"], StringSplitOptions.RemoveEmptyEntries ).Select( p => new Uri( p ) ).ToList();
	}
}
