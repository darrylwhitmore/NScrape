using System;
using System.IO;
using System.Linq;
using NScrape.Interfaces;
using NScrape.Responses;
using Xunit;

// TODO: physically delete deprecated NScrape.Test.Common & NScrape.Test.Framework 

namespace NScrape.Test {
	
	[Trait( "Category", "Integration" )]
	public class WebResponseTestsExplicit {

		[Fact( Explicit = true )]
		public void RedirectWebResponseNoAutoRedirectTest() {
			var webClient = new WebClient();

			var uri = new Uri( "https://jigsaw.w3.org/HTTP/300/301.html" );
			var redirectUri = new Uri( "http://jigsaw.w3.org/HTTP/300/Overview.html" );

			using ( var response = webClient.SendRequest( uri, false ) ) {
				Assert.NotNull( response );
				Assert.True( response.Success );
				Assert.Equal( WebResponseType.Redirect, response.ResponseType );

				var redirectedWebResponse = response as IRedirectedWebResponse;
				Assert.NotNull( redirectedWebResponse );
				Assert.Equal( redirectUri, redirectedWebResponse.RedirectUrl );

				Assert.NotNull( redirectedWebResponse.WebRequest );
				Assert.Equal( uri, redirectedWebResponse.ResponseUrl );
				Assert.Equal( uri, redirectedWebResponse.WebRequest.Destination );
			}
		}

		[Fact( Explicit = true )]
		public void RedirectWebResponseAutoRedirectTest() {
			var webClient = new WebClient();

			var uri = new Uri( "https://jigsaw.w3.org/HTTP/300/301.html" );
			var redirectUri = new Uri( "https://jigsaw.w3.org/HTTP/300/Overview.html" );

			using ( var response = webClient.SendRequest( uri ) ) {
				Assert.NotNull( response );
				Assert.True( response.Success );
				Assert.Equal( WebResponseType.Html, response.ResponseType );
				Assert.Equal( redirectUri, response.ResponseUrl );

				var htmlWebResponse = response as IHtmlWebResponse;
				Assert.NotNull( htmlWebResponse );

				Assert.NotNull( htmlWebResponse.Html );
				Assert.Contains( "A set of HTTP/1.1 redirect codes", htmlWebResponse.Html );
			}
		}

		[Fact( Explicit = true )]
		public void HtmlWebResponseMetaRefreshTagNoAutoRedirectTest() {
			var webClient = new WebClient();

			var uri = new Uri( "https://www.rapidtables.com/web/dev/redirect/html-redirect-test.html" );
			var redirectUri = new Uri( "https://www.rapidtables.com/web/dev/html-redirect.html" );

			using ( var response = webClient.SendRequest( uri, false ) ) {
				Assert.NotNull( response );
				Assert.True( response.Success );
				Assert.Equal( WebResponseType.Redirect, response.ResponseType );

				var redirectedWebResponse = response as IRedirectedWebResponse;
				Assert.NotNull( redirectedWebResponse );
				Assert.Equal( redirectUri, redirectedWebResponse.RedirectUrl );

				Assert.NotNull( redirectedWebResponse.WebRequest );
				Assert.Equal( uri, redirectedWebResponse.ResponseUrl );
				Assert.Equal( uri, redirectedWebResponse.WebRequest.Destination );
			}
		}

		[Fact( Explicit = true )]
		public void HtmlWebResponseMetaRefreshTagAutoRedirectTest() {
			var webClient = new WebClient();

			var uri = new Uri( "https://www.rapidtables.com/web/dev/redirect/html-redirect-test.html" );
			var redirectUri = new Uri( "https://www.rapidtables.com/web/dev/html-redirect.html" );

			using ( var response = webClient.SendRequest( uri ) ) {
				Assert.NotNull( response );
				Assert.True( response.Success );
				Assert.Equal( WebResponseType.Html, response.ResponseType );
				Assert.Equal( redirectUri, response.ResponseUrl );

				var htmlWebResponse = response as IHtmlWebResponse;
				Assert.NotNull( htmlWebResponse );

				Assert.NotNull( htmlWebResponse.Html );
				Assert.Contains( "Press this link to redirect from", htmlWebResponse.Html );
				Assert.Contains( "<em>html-redirect-test.htm</em>", htmlWebResponse.Html );
				Assert.Contains( "back to this page:", htmlWebResponse.Html );
			}
		}

		[Fact( Explicit = true )]
		public void HtmlWebResponseTest() {
			var webClient = new WebClient();

			var uri = new Uri( "https://github.com/darrylwhitmore/NScrape" );

			using ( var response = webClient.SendRequest( uri ) ) {
				Assert.NotNull( response );
				Assert.True( response.Success );
				Assert.Equal( WebResponseType.Html, response.ResponseType );
				Assert.Equal( uri, response.ResponseUrl );

				var htmlWebResponse = response as IHtmlWebResponse;
				Assert.NotNull( htmlWebResponse );

				Assert.NotNull( htmlWebResponse.Html );
				Assert.Contains( "A web scraping framework for .NET", htmlWebResponse.Html );
			}
		}

		[Fact( Explicit = true )]
		public void JsonWebResponseTest() {
			var webClient = new WebClient();

			var uri = new Uri( "http://jsonplaceholder.typicode.com/posts/1/comments" );

			using ( var response = webClient.SendRequest( uri ) ) {
				Assert.NotNull( response );
				Assert.True( response.Success );
				Assert.Equal( WebResponseType.Json, response.ResponseType );
				Assert.Equal( uri, response.ResponseUrl );

				var jsonWebResponse = response as IJsonWebResponse;
				Assert.NotNull( jsonWebResponse );

				Assert.NotNull( jsonWebResponse.Json );
				Assert.Contains( "\"email\": \"Eliseo@gardner.biz\",", jsonWebResponse.Json );
			}
		}

		[Fact( Explicit = true )]
		public void JavaScriptWebResponseTest() {
			var webClient = new WebClient();

			var uri = new Uri( "https://filesampleshub.com/download/code/js/sample1.js" );

			using ( var response = webClient.SendRequest( uri ) ) {
				Assert.NotNull( response );
				Assert.True( response.Success );
				Assert.Equal( WebResponseType.JavaScript, response.ResponseType );
				Assert.Equal( uri, response.ResponseUrl );

				var javaScriptWebResponse = response as IJavaScriptWebResponse;
				Assert.NotNull( javaScriptWebResponse );

				Assert.NotNull( javaScriptWebResponse.JavaScript );
				Assert.Contains( "generateRandomArray", javaScriptWebResponse.JavaScript );
			}
		}
	
		[Fact( Explicit = true )]
		public void XmlWebResponseTest() {
			var webClient = new WebClient();

			var uri = new Uri( "https://www.w3schools.com/js/cd_catalog.xml" );

			using ( var response = webClient.SendRequest( uri ) ) {
				Assert.NotNull( response );
				Assert.True( response.Success );
				Assert.Equal( WebResponseType.Xml, response.ResponseType );
				Assert.Equal( uri, response.ResponseUrl );

				var xmlResponse = response as IXmlWebResponse;
				Assert.NotNull( xmlResponse );

				Assert.NotNull( xmlResponse.XDocument );
				var catalog = xmlResponse.XDocument.Element( "CATALOG" );
				Assert.NotNull( catalog );
				var cds = catalog.Elements( "CD" );
				Assert.NotNull( cds );
				Assert.Equal( 26, cds.Count() );
			}
		}

		[Fact( Explicit = true )]
		public void PlainTextWebResponseTest() {
			var webClient = new WebClient();

			var uri = new Uri( "https://www.cl.cam.ac.uk/~mgk25/ucs/examples/UTF-8-demo.txt" );

			using ( var response = webClient.SendRequest( uri ) ) {
				Assert.NotNull( response );
				Assert.True( response.Success );
				Assert.Equal( WebResponseType.PlainText, response.ResponseType );
				Assert.Equal( uri, response.ResponseUrl );

				var plainTextResponse = response as IPlainTextWebResponse;
				Assert.NotNull( plainTextResponse );

				Assert.NotNull( plainTextResponse.PlainText );
				Assert.Contains( "∮ E⋅da = Q,  n → ∞, ∑ f(i) = ∏ g(i)", plainTextResponse.PlainText );
			}
		}

		[Fact( Explicit = true )]
		public void ImageWebResponseTest() {
			var webClient = new WebClient();

			var uri = new Uri( "https://cpb-us-e1.wpmucdn.com/sites.psu.edu/dist/3/29639/files/2015/10/cat.jpg" );

			using ( var response = webClient.SendRequest( uri ) ) {
				Assert.NotNull( response );
				Assert.True( response.Success );
				Assert.Equal( WebResponseType.Image, response.ResponseType );
				Assert.Equal( uri, response.ResponseUrl );

				var imageResponse = response as IImageWebResponse;
				Assert.NotNull( imageResponse );
				Assert.True( imageResponse.ContentLength > 0 );

				byte[] imageBytes;

				var s = imageResponse.GetImageStream();
				Assert.NotNull( s );

				using ( var ms = new MemoryStream() ) {
					s.CopyTo( ms );

					imageBytes = ms.ToArray();
				}

				Assert.NotNull( imageBytes );

				//File.WriteAllBytes( "catViaStream.jpg", imageBytes );
			}
		}

		[Fact( Explicit = true )]
		public void BinaryWebResponseTest() {
			var webClient = new WebClient();

			var uri = new Uri( "https://hil-speed.hetzner.com/100MB.bin" );

			using ( var response = webClient.SendRequest( uri ) ) {
				Assert.NotNull( response );
				Assert.True( response.Success ); 
				Assert.Equal( WebResponseType.Binary, response.ResponseType );
				Assert.Equal( uri, response.ResponseUrl );

				var binaryResponse = response as IBinaryWebResponse;
				Assert.NotNull( binaryResponse );
				Assert.True( binaryResponse.ContentLength > 0 );

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
	}
}


