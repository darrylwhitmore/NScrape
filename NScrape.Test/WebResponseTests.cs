using System;
using System.IO;
using System.Linq;
using Xunit;

namespace NScrape.Test {
	public class WebResponseTests {

		[Fact]
		public void RedirectWebResponseNoAutoRedirectTest() {
			var webClient = new WebClient();

			var uri = new Uri( "https://jigsaw.w3.org/HTTP/300/301.html" );
			var redirectUri = new Uri( "https://jigsaw.w3.org/HTTP/300/Overview.html" );

			using ( var response = webClient.SendRequest( uri, false ) ) {
				Assert.NotNull( response );
				Assert.True( response.Success );
				Assert.Equal( WebResponseType.Redirect, response.ResponseType );

				var redirectedWebResponse = response as RedirectedWebResponse;
				Assert.NotNull( redirectedWebResponse );
				Assert.Equal( redirectUri, redirectedWebResponse.RedirectUrl );

				Assert.NotNull( redirectedWebResponse.WebRequest );
				Assert.Equal( uri, redirectedWebResponse.ResponseUrl );
				Assert.Equal( uri, redirectedWebResponse.WebRequest.Destination );
			}
		}

		[Fact]
		public void RedirectWebResponseAutoRedirectTest() {
			var webClient = new WebClient();

			var uri = new Uri( "https://jigsaw.w3.org/HTTP/300/301.html" );
			var redirectUri = new Uri( "https://jigsaw.w3.org/HTTP/300/Overview.html" );

			using ( var response = webClient.SendRequest( uri ) ) {
				Assert.NotNull( response );
				Assert.True( response.Success );
				Assert.Equal( WebResponseType.Html, response.ResponseType );
				Assert.Equal( redirectUri, response.ResponseUrl );

				var htmlWebResponse = response as HtmlWebResponse;
				Assert.NotNull( htmlWebResponse );

				Assert.NotNull( htmlWebResponse.Html );
				Assert.Contains( "A set of HTTP/1.1 redirect codes", htmlWebResponse.Html );
			}
		}

		[Fact]
		public void HtmlWebResponseMetaFreshTagNoAutoRedirectTest() {
			var webClient = new WebClient();

			var uri = new Uri( "http://www.pageresource.com/html/refex1.htm" );
			var redirectUri = new Uri( "http://www.pageresource.com/html/refex2.htm" );

			using ( var response = webClient.SendRequest( uri, false ) ) {
				Assert.NotNull( response );
				Assert.True( response.Success );
				Assert.Equal( WebResponseType.Redirect, response.ResponseType );

				var redirectedWebResponse = response as RedirectedWebResponse;
				Assert.NotNull( redirectedWebResponse );
				Assert.Equal( redirectUri, redirectedWebResponse.RedirectUrl );

				Assert.NotNull( redirectedWebResponse.WebRequest );
				Assert.Equal( uri, redirectedWebResponse.ResponseUrl );
				Assert.Equal( uri, redirectedWebResponse.WebRequest.Destination );
			}
		}

		[Fact]
		public void HtmlWebResponseMetaFreshTagAutoRedirectTest() {
			var webClient = new WebClient();

			var uri = new Uri( "http://www.pageresource.com/html/refex1.htm" );
			var redirectUri = new Uri( "http://www.pageresource.com/html/refex2.htm" );

			using ( var response = webClient.SendRequest( uri ) ) {
				Assert.NotNull( response );
				Assert.True( response.Success );
				Assert.Equal( WebResponseType.Html, response.ResponseType );
				Assert.Equal( redirectUri, response.ResponseUrl );

				var htmlWebResponse = response as HtmlWebResponse;
				Assert.NotNull( htmlWebResponse );

				Assert.NotNull( htmlWebResponse.Html );
				Assert.Contains( "The New Page!", htmlWebResponse.Html );
			}
		}

		[Fact]
		public void HtmlWebResponseTest() {
			var webClient = new WebClient();

			var uri = new Uri( "https://github.com/darrylwhitmore/NScrape" );

			using ( var response = webClient.SendRequest( uri ) ) {
				Assert.NotNull( response );
				Assert.True( response.Success );
				Assert.Equal( WebResponseType.Html, response.ResponseType );
				Assert.Equal( uri, response.ResponseUrl );

				var htmlWebResponse = response as HtmlWebResponse;
				Assert.NotNull( htmlWebResponse );

				Assert.NotNull( htmlWebResponse.Html );
				Assert.Contains( "<meta content=\"darrylwhitmore/NScrape\" property=\"og:title\" />", htmlWebResponse.Html );
			}
		}

		[Fact]
		public void JsonWebResponseTest() {
			var webClient = new WebClient();

			var uri = new Uri( "http://jsonplaceholder.typicode.com/posts/1/comments" );

			using ( var response = webClient.SendRequest( uri ) ) {
				Assert.NotNull( response );
				Assert.True( response.Success );
				Assert.Equal( WebResponseType.Json, response.ResponseType );
				Assert.Equal( uri, response.ResponseUrl );

				var jsonWebResponse = response as JsonWebResponse;
				Assert.NotNull( jsonWebResponse );

				Assert.NotNull( jsonWebResponse.Json );
				Assert.Contains( "\"email\": \"Eliseo@gardner.biz\",", jsonWebResponse.Json );
			}
		}

		[Fact]
		public void JavaScriptWebResponseTest() {
			var webClient = new WebClient();

			var uri = new Uri( "http://www.javascriptkit.com/script/script2/offcanvasmenu.js" );

			using ( var response = webClient.SendRequest( uri ) ) {
				Assert.NotNull( response );
				Assert.True( response.Success );
				Assert.Equal( WebResponseType.JavaScript, response.ResponseType );
				Assert.Equal( uri, response.ResponseUrl );

				var javaScriptWebResponse = response as JavaScriptWebResponse;
				Assert.NotNull( javaScriptWebResponse );

				Assert.NotNull( javaScriptWebResponse.JavaScript );
				Assert.Contains( "var offcanvasmenu = (function($){", javaScriptWebResponse.JavaScript );
			}
		}
	
		[Fact]
		public void XmlWebResponseTest() {
			var webClient = new WebClient();

			var uri = new Uri( "http://www.xmlfiles.com/examples/cd_catalog.xml" );

			using ( var response = webClient.SendRequest( uri ) ) {
				Assert.NotNull( response );
				Assert.True( response.Success );
				Assert.Equal( WebResponseType.Xml, response.ResponseType );
				Assert.Equal( uri, response.ResponseUrl );

				var xmlResponse = response as XmlWebResponse;
				Assert.NotNull( xmlResponse );

				Assert.NotNull( xmlResponse.XDocument );
				var catalog = xmlResponse.XDocument.Element( "CATALOG" );
				Assert.NotNull( catalog );
				var cds = catalog.Elements( "CD" );
				Assert.NotNull( cds );
				Assert.Equal( 26, cds.Count() );
			}
		}

		[Fact]
		public void PlainTextWebResponseTest() {
			var webClient = new WebClient();

			var uri = new Uri( "https://www.cl.cam.ac.uk/~mgk25/ucs/examples/UTF-8-demo.txt" );

			using ( var response = webClient.SendRequest( uri ) ) {
				Assert.NotNull( response );
				Assert.True( response.Success );
				Assert.Equal( WebResponseType.PlainText, response.ResponseType );
				Assert.Equal( uri, response.ResponseUrl );

				var plainTextResponse = response as PlainTextWebResponse;
				Assert.NotNull( plainTextResponse );

				Assert.NotNull( plainTextResponse.PlainText );
				Assert.Contains( "∮ E⋅da = Q,  n → ∞, ∑ f(i) = ∏ g(i)", plainTextResponse.PlainText );
			}
		}

		[Fact]
		public void ImageWebResponseTest() {
			var webClient = new WebClient();

			var uri = new Uri( "http://sites.psu.edu/siowfa15/wp-content/uploads/sites/29639/2015/10/cat.jpg" );

			using ( var response = webClient.SendRequest( uri ) ) {
				Assert.NotNull( response );
				Assert.True( response.Success );
				Assert.Equal( WebResponseType.Image, response.ResponseType );
				Assert.Equal( uri, response.ResponseUrl );

				var imageResponse = response as ImageWebResponse;
				Assert.NotNull( imageResponse );

				Assert.NotNull( imageResponse.Image );
			}
		}

		[Fact]
		public void BinaryWebResponseTest() {
			var webClient = new WebClient();

			var uri = new Uri( "https://download-cdn.getsync.com/stable/windows64/BitTorrent-Sync_x64.exe" );

			using ( var response = webClient.SendRequest( uri ) ) {
				Assert.NotNull( response );
				Assert.True( response.Success ); 
				Assert.Equal( WebResponseType.Binary, response.ResponseType );
				Assert.Equal( uri, response.ResponseUrl );

				var binaryResponse = response as BinaryWebResponse;
				Assert.NotNull( binaryResponse );

				byte[] data;

				var s = binaryResponse.GetResponseStream();
				Assert.NotNull( s );

				using ( var ms = new MemoryStream() ) {
					s.CopyTo( ms );

					data = ms.ToArray();
				}

				Assert.NotNull( data );
			}
		}

		[Fact]
		public void BinaryWebResponseDataPropertyBackwardsCompatibilityTest() {
			var webClient = new WebClient();

			var uri = new Uri( "https://download-cdn.getsync.com/stable/windows64/BitTorrent-Sync_x64.exe" );

			using ( var response = webClient.SendRequest( uri ) ) {
				Assert.NotNull( response );
				Assert.True( response.Success );
				Assert.Equal( WebResponseType.Binary, response.ResponseType );
				Assert.Equal( uri, response.ResponseUrl );

				var binaryResponse = response as BinaryWebResponse;
				Assert.NotNull( binaryResponse );

				var data = binaryResponse.Data;
				Assert.NotNull( data );
			}
		}
	}
}


